using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public sealed class Company : IDatabaseModel
    {
        public int Id;
        public string CountryCode;
        public string Name;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO Company (Id, CountryCode, Name)
                  VALUES (@Id, @CountryCode, @Name);",
                new Dictionary<string, object>()
                {
                    { "@Id", Id },
                    { "@CountryCode", CountryCode },
                    { "@Name", Name }
                }
            );
        }
    }
}