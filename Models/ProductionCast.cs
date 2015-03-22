using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public sealed class ProductionCast : IDatabaseModel
    {
        public int ProductionId;
        public int PersonId;
        public int? CharacterId;
        public CharacterRole Role;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO ProductionCast (ProductionId, PersonId, CharacterId, CastRole)
                  VALUES (@ProductionId, @PersonId, @CharacterId, @CastRole);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", ProductionId },
                    { "@PersonId", PersonId },
                    { "@CharacterId", CharacterId },
                    { "@CastRole", Role }
                }
            );
        }
    }
}