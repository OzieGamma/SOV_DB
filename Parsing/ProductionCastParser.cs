using System;
using DB.Models;

namespace DB.Parsing
{
    public sealed class ProductionCastParser : ILineParser<ProductionCast>
    {
        public string FileName
        {
            get { return "Production_Cast"; }
        }

        public ProductionCast Parse( string[] values )
        {
            return new ProductionCast
            {
                ProductionId = ParseUtility.Get( values[0], int.Parse, "ProductionID" ),
                PersonId = ParseUtility.Get( values[1], int.Parse, "PersonID" ),
                CharacterId = ParseUtility.Map( values[2], int.Parse ),
                Role = ParseUtility.Get( values[3], ParseRole, "Role" )
            };
        }

        private static CharacterRole ParseRole( string role )
        {
            switch ( role )
            {
                case "actor":
                    return CharacterRole.Actor;

                case "actress":
                    return CharacterRole.Actress;

                case "cinematographer":
                    return CharacterRole.Cinematographer;

                case "composer":
                    return CharacterRole.Composer;

                case "costume designer":
                    return CharacterRole.CostumeDesigner;

                case "director":
                    return CharacterRole.Director;

                case "editor":
                    return CharacterRole.Editor;

                case "miscellaneous crew":
                    return CharacterRole.MiscellaneousCrew;

                case "producer":
                    return CharacterRole.Producer;

                case "production designer":
                    return CharacterRole.ProductionDesigner;

                case "writer":
                    return CharacterRole.Writer;

                default:
                    throw new InvalidOperationException( "Unknown character role: " + role );
            }
        }
    }
}