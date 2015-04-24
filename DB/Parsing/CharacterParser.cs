using System.Collections.Generic;
using DB.Models;

namespace DB.Parsing
{
    public sealed class CharacterParser : ILineParser
    {
        public string FileName
        {
            get { return "Character"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            yield return new ProductionCharacter
            {
                Id = ParseUtility.Get( values[0], int.Parse, "ID" ),
                Name = ParseUtility.Get( values[1], "Name" )
            };
        }
    }
}