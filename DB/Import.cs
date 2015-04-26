namespace DB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DB.Parsing;

    public class Import
    {
        private readonly string inputFilesDirectory;
        private readonly IOutput output;

        public Import(string inputFilesDirectory, IOutput output)
        {
            this.inputFilesDirectory = inputFilesDirectory;
            this.output = output;
        }

        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 100000;

        public async Task FromCsv()
        {
            var parsers = new ILineParser[] {
                new AlternativePersonNameParser(),
                new AlternativeProductionTitleParser(),
                new CharacterParser(),
                new CompanyParser(),
                new PersonParser(),
                new ProductionParser(),
                new ProductionCastParser(),
                new ProductionCompanyParser(),
            };
            
            this.output.WriteLine("Creating temp dir.");

            var tempDir = new DirectoryInfo( Path.Combine( Path.GetTempPath(), "DB_CSV" ) );
            if ( tempDir.Exists )
            {
                tempDir.Delete( true );
            }
            tempDir.Refresh();
            tempDir.Create();

            this.output.WriteLine("Launching CSV creation");

            Parallel.ForEach( parsers, parser =>
            {
                var results = from obj in this.ParseCsv( parser )
                              group obj by obj.GetType() into grouped
                              select new { Name = grouped.Key.Name, Items = from o in grouped select o.ToString() };

                foreach ( var result in results )
                {
                    var path = Path.Combine( tempDir.FullName, result.Name );
                    File.WriteAllLines( path, result.Items, new UnicodeEncoding( false, true ) );
                }
            } );

            this.output.WriteLine("Done writing CSV.");

            await Database.DisableReferentialIntegrityAsync();
            await Database.DropAllAsync();
            await Database.CreateAllAsync();
            this.output.WriteLine("Done creating DB.");
            this.output.WriteLine("Launching DB import.");

            foreach ( var file in tempDir.EnumerateFiles() )
            {
                string command = string.Format( "BULK INSERT {0} FROM '{1}' WITH ( CODEPAGE = 1200, DATAFILETYPE = 'widechar' )", file.Name, file.FullName );
                await Database.ExecuteAsync( command );
            }

            this.output.WriteLine( "Done." );
        }

        private IEnumerable<object> ParseCsv( ILineParser parser )
        {
            int lineNumber = 0;
            var builder = new StringBuilder();
            foreach ( var values in this.ReadCsv( parser.FileName ) )
            {
                foreach ( var value in parser.Parse( values ) )
                {
                    yield return value;
                }

                lineNumber++;
                if ( lineNumber % ReportPeriod == 0 )
                {
                    Console.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }
            }
        }

        private IEnumerable<string[]> ReadCsv( string fileName )
        {
            var path = Path.Combine(this.inputFilesDirectory, fileName.ToUpper() + ".csv" );

            using ( TextReader reader = new StreamReader( File.OpenRead( path ) ) )
            {
                string line = reader.ReadLine();

                while ( line != null )
                {
                    yield return line.Split( '\t' ).Select( val => val.Trim() ).ToArray();
                    line = reader.ReadLine();
                }
            }
        }
    }
}