using System;
using System.Collections.Generic;
using System.Data;

namespace DBGui.Utilities
{
    public static class DataExtensions
    {
        public static IEnumerable<T> SelectRows<T>( this DataTable table, Func<DataRow, T> selector )
        {
            if ( table.Rows == null )
            {
                yield break;
            }
            foreach ( DataRow row in table.Rows )
            {
                yield return selector( row );
            }
        }

        public static bool HasValue( this DataRow row, string columnName )
        {
            return row[columnName] != DBNull.Value;
        }

        public static string GetString( this DataRow row, string columnName )
        {
            if ( !row.HasValue( columnName ) )
            {
                return null;
            }
            return (string) row[columnName];
        }

        public static int GetInt( this DataRow row, string columnName )
        {
            return (int) row[columnName];
        }

        public static int? GetIntOpt( this DataRow row, string columnName )
        {
            if ( !row.HasValue( columnName ) )
            {
                return null;
            }
            return (int) row[columnName];
        }

        public static DateTimeOffset? GetDateOpt( this DataRow row, string columnName )
        {
            if ( !row.HasValue( columnName ) )
            {
                return null;
            }
            return (DateTimeOffset) new DateTimeOffset( (DateTime) row[columnName] );
        }

        public static T GetEnum<T>( this DataRow row, string columnName )
            where T : struct
        {
            return (T) Enum.Parse( typeof( T ), row.GetString( columnName ) );
        }

        public static T? GetEnumOpt<T>( this DataRow row, string columnName )
            where T : struct
        {
            string value = row.GetString( columnName );
            return value == null ? null : (T?) Enum.Parse( typeof( T ), value );
        }
    }
}