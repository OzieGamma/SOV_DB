using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DB.Parsing;

namespace DB
{
    public static class Program
    {
        // Debug messages will be printed every time this number of lines is parsed
        private const int ReportPeriod = 100000;

        // All CSV files must be in that folder, without renaming them
        private const string CsvRootPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
            @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            ParseCsvs(
                //new AlternativeProductionTitleParser(),
                //new CharacterParser(),
                //new PersonParser(),
                //new ProductionCastParser(),
                //new CompanyParser(),
                //new ProductionCompanyParser(),
                new AlternativePersonNameParser()
            );
            Console.WriteLine( "Done." );
            Console.Read();
        }

        private static void ParseCsvs( params ILineParser<object>[] parsers )
        {
            Task.WaitAll( parsers.Select( parser => Task.Run( () => ParseCsv( parser ) ) ).ToArray() );
        }

        private static IList<T> ParseCsv<T>( ILineParser<T> parser )
            where T : class
        {
            var results = new List<T>();
            var errors = new List<string>();

            int lineNumber = 0;
            foreach ( var values in ReadCsv( parser.FileName ) )
            {
                try
                {
                    results.Add( parser.Parse( values ) );
                }
                catch ( Exception e )
                {
                    errors.Add( string.Format( "[{0}] Exception while parsing line {1}: {2}", parser.FileName, lineNumber, e.Message ) );
                }

                lineNumber++;
                if ( lineNumber % ReportPeriod == 0 )
                {
                    Debug.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }
            }

            if ( errors.Count > 0 )
            {
                throw new Exception( "Parse errors." + Environment.NewLine + string.Join( Environment.NewLine, errors ) );
            }

            return results;
        }

        private static IEnumerable<string[]> ReadCsv( string fileName )
        {
            return File.ReadLines( Path.Combine( CsvRootPath, fileName.ToUpper() + ".csv" ) ).Select( row => row.Split( '\t' ).Select( val => val.Trim() ).ToArray() );
        }
    }
}