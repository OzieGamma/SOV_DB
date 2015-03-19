using DB.Models;

namespace DB.Parsing
{
    public sealed class CharacterParser : LineParser<Character>
    {
        public string FileName
        {
            get { return "Character"; }
        }

        public Character Parse( string[] values )
        {
            return new Character
            {
                Id = ParseUtility.Get( values[0], long.Parse, "ID" ),
                Name = ParseUtility.Get( values[1], "Name" )
            };
        }
    }
}