using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public sealed class AlternativePersonName : IDatabaseModel
    {
        public int PersonId;
        public string Name;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO AlternativePersonName (PersonId, Name)
                  VALUES (@PersonId, @Name);",
                new Dictionary<string, object>()
                {
                    { "@PersonId", PersonId },
                    { "@Name", Name }
                }
            );
        }
    }
}