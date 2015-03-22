using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DB
{
    public static class DatabaseConnection
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Oswald\Documents\GitHub\SOV_DB\DB.mdf;Integrated Security=True";

        public static async Task<SqlDataReader> ExecuteReaderAsync( string queryText, IDictionary<string, object> parameters )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                connection.Open();
                var cmd = new SqlCommand( queryText, connection );

                foreach ( var parameter in parameters )
                {
                    cmd.Parameters.AddWithValue( parameter.Key, parameter.Value ?? DBNull.Value );
                }

                return await cmd.ExecuteReaderAsync();
            }
        }

        public static async Task ExecuteNonQueryAsync( string queryText, IDictionary<string, object> parameters )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                connection.Open();
                var cmd = new SqlCommand( queryText, connection );

                foreach ( var parameter in parameters )
                {
                    cmd.Parameters.AddWithValue( parameter.Key, parameter.Value ?? DBNull.Value );
                }

                int affectedRows = await cmd.ExecuteNonQueryAsync();

                if ( affectedRows == 0 )
                {
                    throw new Exception( "Couldn't insert stuff." );
                }
            }
        }
    }
}