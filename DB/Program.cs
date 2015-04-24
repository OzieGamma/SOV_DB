﻿using System;
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
        private const int ReportPeriod = 10000;
        private const int MaxErrors = 10;

        // All CSV files must be in that folder, without renaming them
        private const string InputFilesPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
        @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private const string OutputFilesPath =
        @"X:\Documents\EPFL\DB\Project dataset output"; // Solal

        private static void Main()
        {
            Console.WriteLine( "Doing work..." );
            ConvertCsvs();
            Console.Read();
        }

        private static void ConvertCsvs()
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


            Parallel.ForEach( parsers, parser =>
            {
                var results = from obj in ParseCsv( parser )
                              group obj by obj.GetType() into grouped
                              select new { Name = grouped.Key.Name, Items = from o in grouped select o.ToString() };

                foreach ( var result in results )
                {
                    var path = Path.Combine( Path.GetTempPath(), result.Name + ".csv" );
                    File.WriteAllLines( path, result.Items, Encoding.UTF8 );
                }
            } );

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

                if ( lineNumber % ReportPeriod == 0 )
                {
                    Console.WriteLine( "[{0}] Done with {1}.", parser.FileName, lineNumber );
                }

                lineNumber++;
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