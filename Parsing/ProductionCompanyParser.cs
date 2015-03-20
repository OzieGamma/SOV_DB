using System;
using DB.Models;

namespace DB.Parsing
{
    public sealed class ProductionCompanyParser : LineParser<ProductionCompany>
    {
        public string FileName
        {
            get { return "Production_Company"; }
        }

        public ProductionCompany Parse( string[] values )
        {
            // values[0] is the ID, we discard it
            return new ProductionCompany
            {
                CompanyId = ParseUtility.Get( values[1], long.Parse, "CompanyID" ),
                ProductionId = ParseUtility.Get( values[2], long.Parse, "ProductionID" ),
                Kind = ParseUtility.Get( values[3], ParseKind, "Kind" )
            };
        }

        private static ProductionCompanyKind ParseKind( string kind )
        {
            switch ( kind )
            {
                case "production companies":
                    return ProductionCompanyKind.ProductionCompany;

                case "distributors":
                    return ProductionCompanyKind.Distributor;

                default:
                    throw new InvalidOperationException( "Unknown production company kind: " + kind );
            }
        }
    }
}