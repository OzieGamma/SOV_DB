using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DB.Models;

namespace DB.Parsing
{
    public sealed class PersonParser : ILineParser<Person>
    {
        public string FileName
        {
            get { return "Person"; }
        }

        public Person Parse( string[] values )
        {
            return new Person
            {
                Id = ParseUtility.Get( values[0], long.Parse, "ID" ),
                Name = ParseUtility.Get( values[1], "Name" ),
                Gender = ParseUtility.Map( values[2], ParseGender ),
                Trivia = ParseUtility.Map( values[3] ),
                Quotes = ParseUtility.Map( values[4] ),
                BirthDate = ParseUtility.Map( values[5], ParseDate ),
                DeathDate = ParseUtility.Map( values[6], ParseDate ),
                BirthName = ParseUtility.Map( values[7] ),
                ShortBio = ParseUtility.Map( values[8] ),
                Spouse = ParseUtility.MapRef( values[9], ParseSpouseInfo ),
                Height = ParseUtility.Map( values[10], ParseHeight )
            };
        }

        private static Gender ParseGender( string gender )
        {
            switch ( gender.ToUpperInvariant() )
            {
                case "M":
                    return Gender.Male;

                case "F":
                    return Gender.Female;

                default:
                    throw new InvalidOperationException( "Unknown gender: " + gender );
            }
        }

        private static decimal ParseHeight( string height )
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

            return realSize;
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

        private static PersonSpouseInfo ParseSpouseInfo( string info )
        {
            string orig = info;
            info = info.Trim();
            info = info.Replace( "  ", " " );
            var spouseInfo = new PersonSpouseInfo();

            if ( info[0] == '\'' )
            {
                info = info.Substring( 1 );
            }

            if ( info.EndsWith( "'" ) )
            {
                spouseInfo.Name = info.Substring( 0, info.Length - 1 );
                return spouseInfo;
            }

            int rparenIndex = orig[0] == '\'' ? info.IndexOf( "' (" ) : info.IndexOf( "(" );
            if ( rparenIndex == -1 ) { rparenIndex = info.IndexOf( "'(" ); }
            spouseInfo.Name = info.Substring( 0, rparenIndex ).Trim();
            info = info.Substring( rparenIndex + ( orig[0] == '\'' ? 1 : 0 ) ).Trim();

            spouseInfo.IsInDatabase = info.StartsWith( "(qv)" );
            if ( spouseInfo.IsInDatabase )
            {
                info = info.Substring( "(qv)".Length ).Trim();
            }

            if ( info.Contains( '-' ) )
            {
                info = info.Substring( 1 ).Trim();

                int dateSepIndex = info.IndexOf( '-' );

                if ( info.Contains( "(?-?) - " ) )
                {
                    spouseInfo.BeginDate = "?";
                    info = info.Substring( 7 ).Trim();
                }
                else
                {
                    spouseInfo.BeginDate = info.Substring( 0, dateSepIndex ).Trim();
                    info = info.Substring( dateSepIndex + 1 ).Trim();
                }
                int endDateIndex = info.IndexOf( ')' );
                if ( endDateIndex == -1 )
                {
                    spouseInfo.EndDate = info.Trim();
                    info = "";
                }
                else
                {
                    spouseInfo.EndDate = info.Substring( 0, endDateIndex ).Trim();
                    info = info.Substring( endDateIndex + 1 ).Trim();
                }
            }

            while ( info.StartsWith( ";" ) || info.StartsWith( "," ) || info.StartsWith( ":" ) )
            {
                info = info.Substring( 1 ).Trim();
            }

            var notes = new List<string>();
            if ( info.Contains( "his death;" ) )
            {
                notes.Add( "his death" );
                info = info.Replace( "his death;", "" ).Trim();
            }
            while ( info.Length > 0 && info[0] == '(' )
            {
                info = info.Substring( 1 ).Trim();
                int endNotesIndex = info.IndexOf( ')' );
                if ( endNotesIndex == -1 )
                {
                    notes.Add( info );
                    info = "";
                    break;
                }
                notes.Add( info.Substring( 0, endNotesIndex ).Trim() );
                info = info.Substring( endNotesIndex + 1 ).Trim();

                while ( info.StartsWith( ";" ) || info.StartsWith( "," ) || info.StartsWith( ":" ) )
                {
                    info = info.Substring( 1 ).Trim();
                }
            }
            if ( notes.Count != 0 )
            {
                spouseInfo.EndNotes = string.Join( ", ", notes );
            }


            if ( info.Length > 0 )
            {
                spouseInfo.ChildrenDescription = info;

                if ( !info.Any( char.IsDigit ) )
                {
                    spouseInfo.ChildrenCount = 1; // e.g. "son, Peter" or "daughter, Alice"
                }
                else if ( info.Contains( "one child born c. 1920" ) || info.Contains( "1908" ) || info.Contains( "son: John, 3/22/62" ) )
                {
                    spouseInfo.ChildrenCount = 1;
                }
                else if ( info.Contains( "twin sons, 1 daughter" ) )
                {
                    spouseInfo.ChildrenCount = 3;
                }
                else if ( info.Contains( "children, 5 sons" ) )
                {
                    spouseInfo.ChildrenCount = 5;
                }
                else
                {
                    info = info.Replace( "at least", "" ).Trim();

                    while ( info.StartsWith( ";" ) || info.StartsWith( "," ) || info.StartsWith( ":" ) )
                    {
                        info = info.Substring( 1 ).Trim();
                    }

                    int spIndex = info.IndexOf( ' ' );
                    spouseInfo.ChildrenCount = int.Parse( info.Substring( 0, spIndex ).Trim() );
                }
            }

            return spouseInfo;
        }
    }
}