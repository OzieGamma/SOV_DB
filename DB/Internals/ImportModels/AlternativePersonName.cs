namespace DB.Internals.ImportModels
{
    internal sealed class AlternativePersonName
    {
        public int PersonId;
        public string Name;

        public override string ToString()
        {
            return string.Join( "\t", PersonId, Name );
        }
    }
}