using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace DB
{
    public static class Database
    {
        private const int Timeout = 1200; // in seconds;

        private const string ConnectionString =
            //  @"Data Source=(LocalDB)\v11.0;Integrated Security=True"; // SQL Server 2012
            @"Data Source=(LocalDB)\mssqllocaldb;Integrated Security=True"; // SQL Server 2014

        public static Task DisableReferentialIntegrityAsync()
        {
            return ExecuteNonQueryAsync( @"EXEC sp_MSForeachTable ""DECLARE @name NVARCHAR (MAX); SET @name = PARSENAME('?', 1); EXEC sp_MSdropconstraints @name""", new Dictionary<string, object>() );
        }

        public static Task DropAllAsync()
        {
            return ExecuteNonQueryAsync( @"EXEC sp_MSForeachTable ""DROP TABLE ?"";", new Dictionary<string, object>() );
        }

        public static Task CreateAllAsync()
        {
            return ExecuteAsync( File.ReadAllText( "create_db.sql" ) );
        }

        public static Task<DataTable> ExecuteQueryAsync( string query )
        {
            return Task.Factory.StartNew( () =>
            {
                var dataTable = new DataTable();
                new SqlDataAdapter( query, ConnectionString ).Fill( dataTable );
                return dataTable;
            } );
        }

        public static async Task ExecuteNonQueryAsync( string command, IDictionary<string, object> parameters )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand( command, connection ) { CommandTimeout = Timeout };

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

        public static async Task ExecuteAsync( string command )
        {
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                await connection.OpenAsync();
                var cmd = new SqlCommand( command, connection ) { CommandTimeout = Timeout };
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}