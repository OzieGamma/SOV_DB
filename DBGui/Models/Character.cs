using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ILookup<ProductionInfo, PersonInfo> AppearsIn { get; private set; }

        public static async Task<Character> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync( "SELECT Id, Name FROM ProductionCharacter WHERE Id = " + id );
            var character = table
                .SelectRows( row =>
                    new Character
                    {
                        Id = row.GetInt( "Id" ),
                        Name = row.GetString( "Name" )
                    } )
                .Single();

            var appearsIn = await Database.ExecuteQueryAsync(
@"SELECT Person.Id AS PersonId, Person.FirstName, Person.LastName, Production.Id AS ProdId, Production.Title, Production.ReleaseYear, Production.Genre
  FROM (ProductionCharacter JOIN ProductionCast ON ProductionCast.CharacterId = ProductionCharacter.Id)
  JOIN Person ON ProductionCast.PersonId = Person.Id
  JOIN Production ON ProductionCast.ProductionId = Production.Id
  WHERE ProductionCharacter.Id = " + id );
            character.AppearsIn = appearsIn
                .SelectRows( row => row )
                .ToLookup(
                    row =>
                        new ProductionInfo(
                            row.GetInt( "ProdId" ),
                            row.GetString( "Title" ),
                            row.GetIntOpt( "ReleaseYear" ),
                            row.GetEnumOpt<ProductionGenre>( "Genre" ) ),
                    row =>
                        new PersonInfo( row.GetInt( "PersonId" ), row.GetString( "FirstName" ), row.GetString( "LastName" ) ) );

            return character;
        }
    }
}