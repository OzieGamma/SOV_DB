namespace DB.Models
{
    public sealed class AlternativeProductionTitle : IDbModel
    {
        public long ProductionId;
        public string Title;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}