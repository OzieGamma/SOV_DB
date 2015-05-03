using System.Collections.Generic;
using DB.Internals.ImportModels;

namespace DB.Internals.Parsing
{
    internal sealed class CompanyParser : ILineParser
    {
        public string FileName
        {
            get { return "Company"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            yield return new Company
            {
                Id = ParseUtility.Get( values[0], int.Parse, "ID" ),
                CountryCode = ParseUtility.MapRef( values[1], ParseCountryCode ),
                Name = ParseUtility.Get( values[2], "Name" )
            };
        }

        private static string ParseCountryCode( string code )
        {
            code = code.Substring( 1, code.Length - 2 );
            if ( code == "xyu" ) // Wrong code for Yugoslavia
            {
                code = "yucs";
            }
            return code;
        }
    }
}