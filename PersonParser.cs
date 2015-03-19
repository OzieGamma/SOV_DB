using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DB.Models;

namespace DB
{
    public static class PersonParser
    {
        public static Person Parse( string[] line )
        {
            var person = new Person();

            person.Id = long.Parse( line[0] );
            person.Name = line[1];
            person.Gender = IsEmpty( line[2] ) ? (Gender?) null : line[2].ToUpper() == "M" ? Gender.Male : Gender.Female;
            if ( !IsEmpty( line[3] ) )
            {
                person.Trivia = line[3].Trim();
            }
            if ( !IsEmpty( line[4] ) )
            {
                person.Quotes = line[4].Trim();
            }

            if ( !IsEmpty( line[5] ) )
            {
                person.BirthDate = ParseDate( line[5] );
            }

            if ( !IsEmpty( line[6] ) )
            {
                person.DeathDate = ParseDate( line[6] );
            }

            if ( !IsEmpty( line[7] ) )
            {
                person.BirthName = line[7].Trim();
            }

            if ( !IsEmpty( line[8] ) )
            {
                person.ShortBio = line[8].Trim();
            }

            if ( !IsEmpty( line[9] ) )
            {
                person.Spouse = GetSpouseInfo( line[9] );
            }

            if ( !IsEmpty( line[10] ) )
            {
                person.Height = GetPersonHeightInMeters( line[10] );
            }
            return person;
        }

        private static decimal GetPersonHeightInMeters( string size )
        {
            string original = size.Trim();
            string s = size.Trim();
            decimal realSize = -1.0m;

            if ( s.Contains( "½" ) )
            {
                s = s.Replace( "½", ".5" );
                //Info( "Replaced a half symbol in " + original );
            }

            if ( s.IndexOf( '\'' ) > -1 && s.IndexOf( '"' ) > -1 && s.IndexOf( '"' ) < s.IndexOf( '\'' ) )
            {
                s = s.Replace( "'", "QUOTE" ).Replace( "\"", "'" ).Replace( "QUOTE", "\"" );
                //Info( "Converted '" + original + "' to: " + s );
            }

            if ( s.Contains( ", " ) )
            {
                s = s.Replace( ", ", " " );
                //Info( "Converted '" + original + "' to '" + s + "', removing the comma" );
            }

            if ( s.EndsWith( "cm" ) )
            {
                realSize = decimal.Parse( s.Replace( "cm", "" ).Trim(), CultureInfo.InvariantCulture ) / 100.0m;
                //Console.WriteLine( "Converted {0} to {1} cm", original, realSize );
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
                            else
                            {
                                //Info( "Expected frac, discarding additional stuff: " + s );
                            }
                        }
                    }
                }


                realSize = ( feet * 12 + inches ) * 2.54m / 100.0m;
                //Console.WriteLine( "Converted {0} to {1}'{2}\" -> {3} cm", original, feet, inches, realSize );
            }
            else
            {
                //Info( "What? " + original + ", assuming cm" );
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
                //Info( "BC date: " + orig );
                date = date.Replace( "BC", "" ).Trim();
            }

            string[] split = date.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
            string actualDate = split[0] + " " + split[1] + " " + split[2];
            if ( date != actualDate )
            {
                //Info( "Converted '" + date + "' to: " + actualDate );
            }

            actualDate = actualDate.TrimEnd( '.' );

            if ( actualDate.Contains( "/98" ) ) // oh come on
            {
                actualDate = actualDate.Replace( "/98", "" );
            }

            DateTimeOffset dto;
            try
            {
                dto = DateTimeOffset.ParseExact( actualDate, "d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None );
            }
            catch
            {
                try
                {
                    // yyy doesn't parse 4-digit years, it doesn't round-trip from ToString
                    dto = DateTimeOffset.ParseExact( actualDate, "d MMMM yyy", CultureInfo.InvariantCulture, DateTimeStyles.None );
                }
                catch
                {

                    // Guess what doesn't parse 3- and 4-digit years?
                    dto = DateTimeOffset.ParseExact( actualDate, "d MMMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None );
                }
            }

            return dto;
        }

        private static PersonSpouseInfo GetSpouseInfo( string col )
        {
            string orig = col;
            col = col.Trim();
            col = col.Replace( "  ", " " );
            var info = new PersonSpouseInfo();

            if ( col[0] == '\'' )
            {
                col = col.Substring( 1 );
            }

            if ( col.EndsWith( "'" ) )
            {
                info.Name = col.Substring( 0, col.Length - 1 );
                return info;
            }

            int rparenIndex = orig[0] == '\'' ? col.IndexOf( "' (" ) : col.IndexOf( "(" );
            if ( rparenIndex == -1 ) { rparenIndex = col.IndexOf( "'(" ); }
            info.Name = col.Substring( 0, rparenIndex ).Trim();
            col = col.Substring( rparenIndex + ( orig[0] == '\'' ? 1 : 0 ) ).Trim();

            info.IsInDatabase = col.StartsWith( "(qv)" );
            if ( info.IsInDatabase )
            {
                col = col.Substring( "(qv)".Length ).Trim();
            }

            if ( col.Contains( '-' ) )
            {
                col = col.Substring( 1 ).Trim();

                int dateSepIndex = col.IndexOf( '-' );

                if ( col.Contains( "(?-?) - " ) )
                {
                    info.BeginDate = "?";
                    col = col.Substring( 7 ).Trim();
                }
                else
                {
                    info.BeginDate = col.Substring( 0, dateSepIndex ).Trim();
                    col = col.Substring( dateSepIndex + 1 ).Trim();
                }
                int endDateIndex = col.IndexOf( ')' );
                if ( endDateIndex == -1 )
                {
                    info.EndDate = col.Trim();
                    col = "";
                }
                else
                {
                    info.EndDate = col.Substring( 0, endDateIndex ).Trim();
                    col = col.Substring( endDateIndex + 1 ).Trim();
                }
            }

            while ( col.StartsWith( ";" ) || col.StartsWith( "," ) || col.StartsWith( ":" ) )
            {
                col = col.Substring( 1 ).Trim();
            }

            var notes = new List<string>();
            if ( col.Contains( "his death;" ) )
            {
                notes.Add( "his death" );
                col = col.Replace( "his death;", "" ).Trim();
            }
            while ( col.Length > 0 && col[0] == '(' )
            {
                col = col.Substring( 1 ).Trim();
                int endNotesIndex = col.IndexOf( ')' );
                if ( endNotesIndex == -1 )
                {
                    notes.Add( col );
                    col = "";
                    break;
                }
                notes.Add( col.Substring( 0, endNotesIndex ).Trim() );
                col = col.Substring( endNotesIndex + 1 ).Trim();

                while ( col.StartsWith( ";" ) || col.StartsWith( "," ) || col.StartsWith( ":" ) )
                {
                    col = col.Substring( 1 ).Trim();
                }
            }
            if ( notes.Count != 0 )
            {
                info.EndNotes = string.Join( ", ", notes );
            }


            if ( col.Length > 0 )
            {
                info.ChildrenDescription = col;

                if ( !col.Any( char.IsDigit ) )
                {
                    info.ChildrenCount = 1; // e.g. "son, Peter" or "daughter, Alice"
                }
                else if ( col.Contains( "one child born c. 1920" ) || col.Contains( "1908" ) || col.Contains( "son: John, 3/22/62" ) )
                {
                    info.ChildrenCount = 1;
                }
                else if ( col.Contains( "twin sons, 1 daughter" ) )
                {
                    info.ChildrenCount = 3;
                }
                else if ( col.Contains( "children, 5 sons" ) )
                {
                    info.ChildrenCount = 5;
                }
                else
                {
                    col = col.Replace( "at least", "" ).Trim();

                    while ( col.StartsWith( ";" ) || col.StartsWith( "," ) || col.StartsWith( ":" ) )
                    {
                        col = col.Substring( 1 ).Trim();
                    }

                    int spIndex = col.IndexOf( ' ' );
                    info.ChildrenCount = int.Parse( col.Substring( 0, spIndex ).Trim() );
                }
            }

            return info;
        }


        private static bool IsEmpty( string s )
        {
            return string.IsNullOrWhiteSpace( s ) || s == "\\N";
        }
    }
}
