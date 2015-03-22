namespace DB.Models
{
    public sealed class Character : IDatabaseModel
    {
        public int Id;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}