using System.Collections.Generic;
using System.Threading.Tasks;
namespace DB.Models
{
    public sealed class AlternativeProductionTitle : IDatabaseModel
    {
        public int ProductionId;
        public string Title;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO AlternativeProductionTitle (ProductionId, Title)
                  VALUES (@ProductionId, @Title);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", ProductionId },
                    { "@Title", Title }
                }
            );
        }
    }
}