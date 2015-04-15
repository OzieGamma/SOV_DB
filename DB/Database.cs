using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace DB
{
    public static class Database
    {
        private const string ConnectionString =
            //  @"Data Source=(LocalDB)\v11.0;Integrated Security=True"; // SQL Server 2012
            @"Data Source=(LocalDB)\mssqllocaldb;Integrated Security=True"; // SQL Server 2014

        public static Task DisableReferentialIntegrityAsync()
        {
            return ExecuteNonQueryAsync( @"EXEC sp_MSForeachTable ""DECLARE @name NVARCHAR (MAX); SET @name = PARSENAME('?', 1); EXEC sp_MSdropconstraints @name""", new Dictionary<string, object>() );
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync( string query, IDictionary<string, object> parameters )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand( query, connection );

                foreach ( var parameter in parameters )
                {
                    cmd.Parameters.AddWithValue( parameter.Key, parameter.Value ?? DBNull.Value );
                }

                return await cmd.ExecuteReaderAsync();
            }
        }

        public static async Task ExecuteNonQueryAsync( string command, IDictionary<string, object> parameters )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand( command, connection );

                foreach ( var parameter in parameters )
                {
                    object value;
                    if ( parameter.Value == null )
                    {
                        value = DBNull.Value;
                    }
                    else
                    {
                        var formattable = parameter.Value as IFormattable;
                        if ( formattable == null )
                        {
                            value = parameter.Value.ToString();
                        }
                        else
                        {
                            value = formattable.ToString( null, CultureInfo.InvariantCulture );
                        }
                    }
                    cmd.Parameters.AddWithValue( parameter.Key, value );
                }

                int affectedRows = await cmd.ExecuteNonQueryAsync();

                if ( affectedRows == 0 )
                {
                    throw new Exception( "Couldn't insert stuff." );
                }
            }
        }

        public static async Task ExecuteCreateAsync( string command )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                await connection.OpenAsync();
                await new SqlCommand( command, connection ).ExecuteNonQueryAsync();
            }
        }
    }
}