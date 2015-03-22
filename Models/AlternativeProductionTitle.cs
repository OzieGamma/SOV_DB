namespace DB.Models
{
    public sealed class AlternativeProductionTitle : IDatabaseModel
    {
        public int ProductionId;
        public string Title;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}