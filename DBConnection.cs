namespace DB
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal static class DBConnection
    {
        private const string ConnectionString =
            @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Oswald\Documents\GitHub\SOV_DB\DB.mdf;Integrated Security=True";

        public static async Task<SqlDataReader> ExecuteReader(string queryText, IDictionary<string, object> parameters)
        {
            // Close automatically called
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(queryText, connection);

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                return await cmd.ExecuteReaderAsync();
            }
        }

        public static async Task<int> ExecuteNonQuery(string queryText, Dictionary<string, object> parameters)
        {
            // Close automatically called
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(queryText, connection);

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
