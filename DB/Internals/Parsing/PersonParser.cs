﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DB.Internals.ImportModels;
using DB.Models;

namespace DB.Internals.Parsing
{
    internal sealed class PersonParser : ILineParser
    {
        public string FileName
        {
            get { return "Person"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            Tuple<string, string> firstLast = ParseUtility.Get( values[1], ParseName, "Name" );
            yield return new Person
            {
                Id = ParseUtility.Get( values[0], int.Parse, "ID" ),
                FirstName = firstLast.Item1,
                LastName = firstLast.Item2,
                Gender = ParseUtility.Map( values[2], ParseGender ),
                Trivia = ParseUtility.Map( values[3] ),
                Quotes = ParseUtility.Map( values[4] ),
                BirthDate = ParseUtility.Map( values[5], ParseDate ),
                DeathDate = ParseUtility.Map( values[6], ParseDate ),
                BirthName = ParseUtility.Map( values[7] ),
                ShortBio = ParseUtility.Map( values[8] ),
                SpouseInfo = ParseUtility.Map( values[9] ),
                Height = ParseUtility.Map( values[10], ParseHeight )
            };
        }

        private static Gender ParseGender( string gender )
        {
            switch ( gender.ToUpperInvariant() )
            {
                case "M":
                    return Gender.M;

                case "F":
                    return Gender.F;

                default:
                    throw new InvalidOperationException( "Unknown gender: " + gender );
            }
        }

        private static Tuple<string, string> ParseName( string name )
        {
            if ( name == "Lee, Mido, Chia Jung" )
            {
                name = "Chia Jung Lee, Mido";
            }

            string[] parts = name.Split( ',' ).Where( s => !string.IsNullOrWhiteSpace( s ) ).ToArray();

            if ( parts.Length == 1 )
            {
                return Tuple.Create( name.Trim(), (string) null );
            }
            if ( parts.Length == 2 )
            {
                return Tuple.Create( parts[1].Trim(), parts[0].Trim() );
            }

            throw new Exception( "Too many parts" );
        }

        private static int ParseHeight( string height )
        {
            // HERE BE DRAGONS!

            string original = height.Trim();
            string s = height.Trim();
            decimal realSize = -1.0m;

            if ( s.Contains( "½" ) )
            {
                s = s.Replace( "½", ".5" );
            }

            if ( s.IndexOf( '\'' ) > -1 && s.IndexOf( '"' ) > -1 && s.IndexOf( '"' ) < s.IndexOf( '\'' ) )
            {
                s = s.Replace( "'", "QUOTE" ).Replace( "\"", "'" ).Replace( "QUOTE", "\"" );
            }

            if ( s.Contains( ", " ) )
            {
                s = s.Replace( ", ", " " );
            }

            if ( s.EndsWith( "cm" ) )
            {
                realSize = decimal.Parse( s.Replace( "cm", "" ).Trim(), CultureInfo.InvariantCulture ) / 100.0m;
            }
            else if ( s.Contains( "'" ) )
            {
                var split = s.Split( '\'' );
                decimal feet = decimal.Parse( split[0] );
                decimal inches = 0.0m;
                if ( split.Length > 1 && !string.IsNullOrWhiteSpace( split[1] ) )
                {
                    split = split[1].Split( new[] { "\"", " " }, StringSplitOptions.RemoveEmptyEntries );
                    if ( split[0].Trim().Contains( "/" ) )
                    {
                        var frac = split[0].Trim().Split( '/' );
                        feet += decimal.Parse( frac[0].Trim() ) / decimal.Parse( frac[1].Trim() );
                    }
                    else
                    {
                        inches = int.Parse( split[0] );
                        if ( split.Length > 1 )
                        {
                            if ( split[1].Contains( "/" ) )
                            {
                                var frac = split[1].Trim().Split( '/' );
                                inches += decimal.Parse( frac[0].Trim() ) / decimal.Parse( frac[1].Trim() );
                            }
                        }
                    }
                }

                realSize = ( feet * 12 + inches ) * 2.54m / 100.0m;
            }
            else
            {
                realSize = decimal.Parse( s ) / 100.0m;
            }

            return (int) ( realSize * 100.0m );
        }

        private static DateTimeOffset ParseDate( string date )
        {
            string orig = date;

            // BEWARE: We need something special to handle that
            if ( date.EndsWith( "BC" ) )
            {
                date = date.Replace( "BC", "" ).Trim();
            }

            string[] split = date.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
            string actualDate = split[0] + " " + split[1] + " " + split[2];
            actualDate = actualDate.TrimEnd( '.' );

            if ( actualDate.Contains( "/98" ) ) // oh come on
            {
                actualDate = actualDate.Replace( "/98", "" );
            }

            DateTimeOffset dto;
            if ( !DateTimeOffset.TryParseExact( actualDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dto ) )
            {
                // yyy doesn't parse 4-digit years, it doesn't round-trip from ToString
                if ( !DateTimeOffset.TryParseExact( actualDate, "d MMMM yyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dto ) )
                {

                    // Guess what doesn't parse 3- and 4-digit years?
                    dto = DateTimeOffset.ParseExact( actualDate, "d MMMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None );
                }
            }

            return dto;
        }
    }
}