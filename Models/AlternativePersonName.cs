namespace DB.Models
{
    public sealed class AlternativePersonName : IDbModel
    {
        public long PersonID;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}