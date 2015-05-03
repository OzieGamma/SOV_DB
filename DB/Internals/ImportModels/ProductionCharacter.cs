namespace DB.Internals.ImportModels
{
    internal sealed class ProductionCharacter
    {
        public int Id;
        public string Name;

        public override string ToString()
        {
            return string.Join( "\t", Id, Name );
        }
    }
}