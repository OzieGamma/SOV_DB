namespace DB
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DB.Parsing;

    public class Import
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 100000;

        private readonly IOutput output;

        public Import(IOutput output)
        {
            this.output = output;
        }

        public async Task FromCsvDirectory(string directoryPath)
        {
            var parsers = new ILineParser[]
                              {
                                  new AlternativePersonNameParser(), new AlternativeProductionTitleParser(),
                                  new CharacterParser(), new CompanyParser(), new PersonParser(), new ProductionParser(),
                                  new ProductionCastParser(), new ProductionCompanyParser()
                              };

            this.output.WriteLine("Creating temp dirs");

            var tempDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "DB_CSV"));
            if (tempDir.Exists)
            {
                tempDir.Delete(true);
            }
            tempDir.Refresh();
            tempDir.Create();

            this.output.WriteLine("Writing CSV");

            Parallel.ForEach(
                parsers,
                parser =>
                    {
                        var results = from obj in this.ParseCsv(directoryPath, parser)
                                      group obj by obj.GetType()
                                      into grouped
                                      select new { grouped.Key.Name, Items = from o in grouped select o.ToString() };

                        foreach (var result in results)
                        {
                            string path = Path.Combine(tempDir.FullName, result.Name);
                            File.WriteAllLines(path, result.Items, new UnicodeEncoding(false, true));

                            this.output.WriteLine("Done writing CSV for {0}", result.Name);
                        }
                    });

            await Database.DisableReferentialIntegrityAsync();
            await Database.DropAllAsync();
            await Database.CreateAllAsync();

            this.output.WriteLine("Inserting into DB");

            foreach (var file in tempDir.EnumerateFiles())
            {
                string command =
                    string.Format(
                        "BULK INSERT {0} FROM '{1}' WITH ( CODEPAGE = 1200, DATAFILETYPE = 'widechar' )",
                        file.Name,
                        file.FullName);
                await Database.ExecuteAsync(command);
                this.output.WriteLine("Done inserting into DB {0}", file);
            }

            this.output.WriteLine("Done.");
        }

        private IEnumerable<object> ParseCsv(string directoryPath, ILineParser parser)
        {
            int lineNumber = 0;
            var builder = new StringBuilder();
            foreach (var values in this.ReadCsv(directoryPath, parser.FileName))
            {
                foreach (var value in parser.Parse(values))
                {
                    yield return value;
                }

                lineNumber++;
                if (lineNumber % ReportPeriod == 0)
                {
                    this.output.WriteLine("[{0}] Done with {1}.", parser.FileName, lineNumber);
                }
            }
        }

        private IEnumerable<string[]> ReadCsv(string directoryPath, string fileName)
        {
            string path = Path.Combine(directoryPath, fileName.ToUpper() + ".csv");

            using (TextReader reader = new StreamReader(File.OpenRead(path)))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    yield return line.Split('\t').Select(val => val.Trim()).ToArray();
                    line = reader.ReadLine();
                }
            }
        }
    }
}
