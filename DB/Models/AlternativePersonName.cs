namespace DB.Models
{
    public sealed class AlternativePersonName
    {
        public int PersonId;
        public string Name;

        public override string ToString()
        {
            return string.Join( "\t", PersonId, Name );
        }
    }
}