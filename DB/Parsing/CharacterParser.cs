using DB.Models;

namespace DB.Parsing
{
    public sealed class CharacterParser : ILineParser<Character>
    {
        public string FileName
        {
            get { return "Character"; }
        }

        public Character Parse( string[] values )
        {
            return new Character
            {
                Id = ParseUtility.Get( values[0], int.Parse, "ID" ),
                Name = ParseUtility.Get( values[1], "Name" )
            };
        }
    }
}