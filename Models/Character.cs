namespace DB.Models
{
    public sealed class Character : IDbModel
    {
        public long Id;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}