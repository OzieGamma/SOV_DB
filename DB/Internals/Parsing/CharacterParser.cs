using System.Collections.Generic;
using DB.Internals.ImportModels;

namespace DB.Internals.Parsing
{
    internal sealed class CharacterParser : ILineParser
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