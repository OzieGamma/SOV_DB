namespace DB.Models
{
    public sealed class Company : IDbModel
    {
        public long Id;
        public string CountryCode;
        public string Name;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}