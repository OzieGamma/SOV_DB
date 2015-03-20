using DB.Models;

namespace DB.Parsing
{
    public sealed class CompanyParser : LineParser<Company>
    {
        public string FileName
        {
            get { return "Company"; }
        }

        public Company Parse( string[] values )
        {
            return new Company
            {
                Id = ParseUtility.Get( values[0], long.Parse, "ID" ),
                CountryCode = ParseUtility.GetOrDefault( values[1], ParseCountryCode ),
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