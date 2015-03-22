namespace DB.Models
{
    public sealed class Company : IDatabaseModel
    {
        public int Id;
        public string CountryCode;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}