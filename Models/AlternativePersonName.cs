namespace DB.Models
{
    public sealed class AlternativePersonName : IDatabaseModel
    {
        public int PersonId;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}