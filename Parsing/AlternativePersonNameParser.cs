using DB.Models;

namespace DB.Parsing
{
    public sealed class AlternativePersonNameParser : ILineParser<AlternativePersonName>
    {
        public string FileName
        {
            get { return "Alternative_Name"; }
        }

        public AlternativePersonName Parse( string[] values )
        {
            // values [0] is the ID, which we discard
            return new AlternativePersonName
            {
                PersonID = ParseUtility.Get( values[1], long.Parse, "PersonID" ),
                Name = ParseUtility.Get( values[2], "Name" )
            };
        }
    }
}