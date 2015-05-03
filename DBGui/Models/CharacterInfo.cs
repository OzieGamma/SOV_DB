namespace DBGui.Models
{
    public sealed class CharacterInfo
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CharacterInfo( int id, string name )
        {
            Id = id;
            Name = name;
        }
    }
}