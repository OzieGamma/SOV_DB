using System.Collections.Generic;
using DB.Internals.ImportModels;

namespace DB.Internals.Parsing
{
    internal sealed class AlternativePersonNameParser : ILineParser
    {
        public string FileName
        {
            get { return "Alternative_Name"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            // values [0] is the ID, which we discard
            yield return new AlternativePersonName
            {
                PersonId = ParseUtility.Get( values[1], int.Parse, "PersonID" ),
                Name = ParseUtility.Get( values[2], "Name" )
            };
        }
    }
}