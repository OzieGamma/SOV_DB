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

        public static string GetString( this DataRow row, string columnName )
        {
            if ( row[columnName] == DBNull.Value )
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
            if ( row[columnName] == DBNull.Value )
            {
                return null;
            }
            return (int) row[columnName];
        }

        public static T? GetEnum<T>( this DataRow row, string columnName )
            where T : struct
        {
            string value = row.GetString( columnName );
            return columnName == null ? null : (T?) Enum.Parse( typeof( T ), value );
        }
    }
}