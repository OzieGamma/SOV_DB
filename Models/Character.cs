using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public sealed class Character : IDatabaseModel
    {
        public int Id;
        public string Name;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO ProductionCharacter (Id, Name)
                  VALUES (@Id, @Name);",
                new Dictionary<string, object>()
                {
                    { "@Id", Id },
                    { "@Name", Name }
                }
            );
        }
    }
}