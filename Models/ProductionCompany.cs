using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public sealed class ProductionCompany : IDatabaseModel
    {
        public int ProductionId;
        public int CompanyId;
        public ProductionCompanyKind Kind;

        public Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO ProductionCompany (ProductionId, CompanyId, Kind)
                  VALUES (@ProductionId, @CompanyId, @Kind);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", ProductionId },
                    { "@CompanyId", CompanyId },
                    { "@Kind", Kind }
                }
            );
        }
    }
}