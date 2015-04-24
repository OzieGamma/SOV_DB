namespace DB.Models
{
    public sealed class ProductionCompany
    {
        public int ProductionId;
        public int CompanyId;
        public ProductionCompanyKind Kind;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, CompanyId, Kind );
        }
    }
}