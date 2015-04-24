using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DB.Parsing;

namespace DB
{
    using System.Text;
    using System.Threading.Tasks;

    public static class Program
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 100000;

        // All CSV files must be in that folder, without renaming them
        private const string InputFilesPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
        @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            Console.WriteLine( "Doing work..." );
            ConvertCsvs();
            Console.Read();
        }

        private static async void ConvertCsvs()
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

            var tempDir = new DirectoryInfo( Path.Combine( Path.GetTempPath(), "DB_CSV" ) );
            if ( tempDir.Exists )
            {
                tempDir.Delete( true );
            }
            tempDir.Create();

            Parallel.ForEach( parsers, parser =>
            {
                var results = from obj in ParseCsv( parser )
                              group obj by obj.GetType() into grouped
                              select new { Name = grouped.Key.Name, Items = from o in grouped select o.ToString() };

                foreach ( var result in results )
                {
                    var path = Path.Combine( tempDir.FullName, result.Name );
                    File.WriteAllLines( path, result.Items, new UTF8Encoding( false ) );
                }
            } );

            await Database.DisableReferentialIntegrityAsync();
            await Database.DropAllAsync();
            await Database.CreateAllAsync();
            foreach ( var file in tempDir.EnumerateFiles() )
            {
                string command = string.Format( "BULK INSERT {0} FROM '{1}'", file.Name, file.FullName );
                await Database.ExecuteAsync( command );
            }

            Console.WriteLine( "Done." );
        }

        private static IEnumerable<object> ParseCsv( ILineParser parser )
        {
            int lineNumber = 0;
            var builder = new StringBuilder();
            foreach ( var values in ReadCsv( parser.FileName ) )
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

        private static IEnumerable<string[]> ReadCsv( string fileName )
        {
            var path = Path.Combine( InputFilesPath, fileName.ToUpper() + ".csv" );

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