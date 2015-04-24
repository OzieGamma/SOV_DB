using System.Collections.Generic;
using DB.Models;

namespace DB.Parsing
{
    public sealed class AlternativeProductionTitleParser : ILineParser
    {
        public string FileName
        {
            get { return "Alternative_Title"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            // values[0] is the ID, we discard it
            yield return new AlternativeProductionTitle
            {
                ProductionId = ParseUtility.Get( values[1], int.Parse, "ProductionID" ),
                Title = ParseUtility.Get( values[2], "Title" )
            };
        }
    }
}