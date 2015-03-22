namespace DB.Models
{
    public sealed class ProductionCompany : IDatabaseModel
    {
        public int ProductionId;
        public int CompanyId;
        public ProductionCompanyKind Kind;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}