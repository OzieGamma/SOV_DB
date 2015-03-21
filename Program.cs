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
        private const int ReportPeriod = 10000;

        // All CSV files must be in that folder, without renaming them
        private const string CsvRootPath =
            @"C:\Users\Oswald\Downloads\Movies"; // Oswald
            //@"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            ParseCsvs(
                //new AlternativeProductionTitleParser(),
                //new CharacterParser(),
                new PersonParser()
                //new ProductionCastParser(),
                //new CompanyParser(),
                //new ProductionCompanyParser(),
                //new AlternativePersonNameParser()
            );

            Console.WriteLine( "Done." );
            Console.Read();
        }

        private static void ParseCsvs( params ILineParser<IDbModel>[] parsers )
        {
            Task.WaitAll( parsers.Select( parser => Task.Run( () => ParseCsv( parser ) ) ).ToArray() );
        }

        private static void ParseCsv<T>( ILineParser<T> parser)
            where T : IDbModel
        {
            var errors = new List<string>();

            int lineNumber = 0;
            foreach ( var values in ReadCsv( parser.FileName ) )
            {
                try
                {
                    parser.Parse( values ).InsertIntoDb();
                }
                catch ( Exception e )
                {
                    errors.Add( string.Format( "[{0}] Exception while parsing line {1}: {2}", parser.FileName, lineNumber, e.Message ) );
                }

                lineNumber++;
                if ( lineNumber % ReportPeriod == 0 )
                {
                    Console.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }
            }

            if ( errors.Count > 0 )
            {
                throw new Exception( "Parse errors." + Environment.NewLine + string.Join( Environment.NewLine, errors ) );
            }
        }

        private static IEnumerable<string[]> ReadCsv( string fileName )
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