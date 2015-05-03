using System;
using System.Collections.Generic;
using DB.Internals.ImportModels;
using DB.Models;

namespace DB.Internals.Parsing
{
    internal sealed class ProductionCastParser : ILineParser
    {
        public string FileName
        {
            get { return "Production_Cast"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            yield return new ProductionCast
            {
                ProductionId = ParseUtility.Get( values[0], int.Parse, "ProductionID" ),
                PersonId = ParseUtility.Get( values[1], int.Parse, "PersonID" ),
                CharacterId = ParseUtility.Map( values[2], int.Parse ),
                Role = ParseUtility.Get( values[3], ParseRole, "Role" )
            };
        }

        private static PersonRole ParseRole( string role )
        {
            switch ( role )
            {
                case "actor":
                    return PersonRole.Actor;

                case "actress":
                    return PersonRole.Actress;

                case "cinematographer":
                    return PersonRole.Cinematographer;

                case "composer":
                    return PersonRole.Composer;

                case "costume designer":
                    return PersonRole.CostumeDesigner;

                case "director":
                    return PersonRole.Director;

                case "editor":
                    return PersonRole.Editor;

                case "miscellaneous crew":
                    return PersonRole.MiscellaneousCrew;

                case "producer":
                    return PersonRole.Producer;

                case "production designer":
                    return PersonRole.ProductionDesigner;

                case "writer":
                    return PersonRole.Writer;

                default:
                    throw new InvalidOperationException( "Unknown character role: " + role );
            }
        }
    }
}