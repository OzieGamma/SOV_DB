using System;
using System.Collections.Generic;
using DB.Internals.ImportModels;
using DB.Models;

namespace DB.Internals.Parsing
{
    internal sealed class ProductionCompanyParser : ILineParser
    {
        public string FileName
        {
            get { return "Production_Company"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            // values[0] is the ID, we discard it
            yield return new ProductionCompany
            {
                CompanyId = ParseUtility.Get( values[1], int.Parse, "CompanyID" ),
                ProductionId = ParseUtility.Get( values[2], int.Parse, "ProductionID" ),
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