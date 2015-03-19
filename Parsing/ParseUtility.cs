using System;

namespace DB.Parsing
{
    public static class ParseUtility
    {
        public static string Get( string s, string name )
        {
            EnsureNotEmpty( s, name );
            return s;
        }

        public static T Get<T>( string s, Func<string, T> selector, string name )
        {
            EnsureNotEmpty( s, name );
            return selector( s );
        }

        public static string GetOrDefault( string s )
        {
            return IsEmpty( s ) ? null : s;
        }

        public static T GetOrDefault<T>( string s, Func<string, T> selector )
        {
            return IsEmpty( s ) ? default( T ) : selector( s );
        }

        private static bool IsEmpty( string s )
        {
            return string.IsNullOrWhiteSpace( s ) || s == "\\N";
        }

        private static void EnsureNotEmpty( string s, string name )
        {
            if ( IsEmpty( s ) )
            {
                throw new Exception( name + " must not be empty." );
            }
        }
    }
}