namespace DB.Models
{
    public sealed class AlternativeProductionTitle
    {
        public int ProductionId;
        public string Title;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, Title );
        }
    }
}