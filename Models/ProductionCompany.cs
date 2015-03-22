namespace DB.Models
{
    public sealed class ProductionCompany : IDbModel
    {
        public long ProductionId;
        public long CompanyId;
        public ProductionCompanyKind Kind;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}