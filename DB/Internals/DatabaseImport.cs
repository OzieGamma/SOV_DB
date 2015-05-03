using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Internals.Parsing;

namespace DB.Internals
{
    internal static class DatabaseImport
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 100000;

        public static async Task ImportFromDirectoryAsync( string directoryPath )
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
            if ( !tempDir.Exists )
            {
                tempDir.Create();
            }

            Parallel.ForEach( parsers, parser =>
            {
                var results = from obj in ParseCsv( directoryPath, parser )
                              group obj by obj.GetType() into grouped
                              select new { Name = grouped.Key.Name, Items = from o in grouped select o.ToString() };

                foreach ( var result in results )
                {
                    var path = Path.Combine( tempDir.FullName, result.Name );
                    File.WriteAllLines( path, result.Items, new UnicodeEncoding( false, true ) );
                }
            } );

            await Database.DisableReferentialIntegrityAsync();
            await Database.DropAllAsync();
            await Database.CreateAllAsync();
            foreach ( var file in tempDir.EnumerateFiles() )
            {
                string command = string.Format( "BULK INSERT {0} FROM '{1}' WITH ( CODEPAGE = 1200, DATAFILETYPE = 'widechar' )", file.Name, file.FullName );
                await Database.ExecuteAsync( command );
            }

            Debug.WriteLine( "Done." );
        }

        private static IEnumerable<object> ParseCsv( string directoryPath, ILineParser parser )
        {
            int lineNumber = 0;
            var builder = new StringBuilder();
            foreach ( var values in ReadCsv( directoryPath, parser.FileName ) )
            {
                foreach ( var value in parser.Parse( values ) )
                {
                    yield return value;
                }

                lineNumber++;
                if ( lineNumber % ReportPeriod == 0 )
                {
                    Debug.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }
            }
        }

        private static IEnumerable<string[]> ReadCsv( string directoryPath, string fileName )
        {
            var path = Path.Combine( directoryPath, fileName.ToUpper() + ".csv" );

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