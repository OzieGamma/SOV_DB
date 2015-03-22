using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DB.Parsing;

namespace DB
{
    public static class Program
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 10000;
        private const int MaxErrors = 10;

        // All CSV files must be in that folder, without renaming them
        private const string CsvRootPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
            @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            Console.WriteLine( "Doing work..." );
            DoWork();
            Console.Read();
        }

        private static async void DoWork()
        {
            await Database.ExecuteCreateAsync( File.ReadAllText( "create_db.sql" ) );
            await Database.DisableReferentialIntegrityAsync();

            var parsers = new ILineParser<IDatabaseModel>[] {
                //new AlternativePersonNameParser(),
                //new AlternativeProductionTitleParser(),
                //new CharacterParser(),
                //new CompanyParser(),
                new PersonParser(),
                //new ProductionParser(),
                //new ProductionCastParser(),
                //new ProductionCompanyParser(),
            };

            foreach ( var model in parsers.AsParallel().Select( ParseCsv ).SelectMany( x => x ) )
            {
                await model.InsertInDatabaseAsync();
            }

            Console.WriteLine( "Done." );
        }

        private static IList<IDatabaseModel> ParseCsv( ILineParser<IDatabaseModel> parser )
        {
            var results = new List<IDatabaseModel>();
            var errors = new List<Exception>();

            int lineNumber = 0;
            foreach ( var values in ReadCsv( parser.FileName ) )
            {
                try
                {
                    results.Add( parser.Parse( values ) );
                }
                catch ( Exception e )
                {
                    errors.Add( e );
                }

                lineNumber++;
                if ( lineNumber % ReportPeriod == 0 )
                {
                    Debug.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }

                if ( errors.Count >= MaxErrors )
                {
                    break;
                }
            }

            if ( errors.Count > 0 )
            {
                throw new AggregateException( errors );
            }

            return results;
        }

        private static IEnumerable<string[]> ReadCsv( string fileName )
        {
            var path = Path.Combine( CsvRootPath, fileName.ToUpper() + ".csv" );

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