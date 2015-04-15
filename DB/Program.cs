using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DB.Parsing;

namespace DB
{
    using System.Threading.Tasks;

    public static class Program
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 10000;
        private const int MaxErrors = 10;

        // All CSV files must be in that folder, without renaming them
        private const string CsvRootPath =
            @"C:\Users\Oswald\Downloads\Movies"; // Oswald
        //@"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            Console.WriteLine("Doing work...");
            DoWork();
            Console.Read();
        }

        private static async void DoWork()
        {
            await Database.ExecuteCreateAsync(File.ReadAllText("create_db.sql"));
            await Database.DisableReferentialIntegrityAsync();

            var parsers = new ILineParser<IDatabaseModel>[] {
                new AlternativePersonNameParser(),
                new AlternativeProductionTitleParser(),
                new CharacterParser(),
                new CompanyParser(),
                new PersonParser(),
                new ProductionParser(),
                new ProductionCastParser(),
                new ProductionCompanyParser(),
            };

            Task.WaitAll(parsers.AsParallel().SelectMany(ParseCsv).ToArray());

            Console.WriteLine("Done.");
        }

        private static IList<Task> ParseCsv(ILineParser<IDatabaseModel> parser)
        {
            var errors = new List<Exception>();
            var tasks = new List<Task>();

            int lineNumber = 0;
            foreach ( var values in ReadCsv( parser.FileName ) )
            {
                try
                {
                    var task = parser.Parse(values).InsertInDatabaseAsync();
                    if (lineNumber % ReportPeriod == 0)
                    {
                        var message = string.Format("[{0}] Done with {1}.", parser.FileName, lineNumber);
                        task = task.ContinueWith(_ => Console.WriteLine(message));
                    }

                    tasks.Add(task);
                }
                catch ( Exception e )
                {
                    errors.Add( e );
                }

                lineNumber += 1;

                if ( errors.Count >= MaxErrors )
                {
                    break;
                }
            }

            if ( errors.Count > 0 )
            {
                throw new AggregateException( errors );
            }

            return tasks;
        }

        private static IEnumerable<string[]> ReadCsv(string fileName)
        {
            var path = Path.Combine(CsvRootPath, fileName.ToUpper() + ".csv");

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