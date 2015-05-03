using System;
using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class Person
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Gender? Gender { get; private set; }
        public string Trivia { get; private set; }
        public string Quotes { get; private set; }
        public DateTimeOffset? BirthDate { get; private set; }
        public DateTimeOffset? DeathDate { get; private set; }
        public string BirthName { get; private set; }
        public string ShortBio { get; private set; }
        public string SpouseInfo { get; private set; }
        public int? Height { get; private set; }

        public string[] AlternativeNames { get; private set; }
        public ILookup<ProductionInfo, PersonRoleInfo> Roles { get; private set; }

        public static async Task<Person> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync( "SELECT Id, FirstName, LastName, Gender, Trivia, Quotes, BirthDate, DeathDate, BirthName, ShortBio, SpouseInfo, Height FROM Person WHERE Id = " + id );
            var person = table.SelectRows( row =>
                new Person
                {
                    Id = row.GetInt( "Id" ),
                    FirstName = row.GetString( "FirstName" ),
                    LastName = row.GetString( "LastName" ),
                    Gender = row.GetEnumOpt<Gender>( "Gender" ),
                    Trivia = row.GetString( "Trivia" ),
                    Quotes = row.GetString( "Quotes" ),
                    BirthDate = row.GetDateOpt( "BirthDate" ),
                    DeathDate = row.GetDateOpt( "DeathDate" ),
                    BirthName = row.GetString( "BirthName" ),
                    ShortBio = row.GetString( "ShortBio" ),
                    SpouseInfo = row.GetString( "SpouseInfo" ),
                    Height = row.GetIntOpt( "Height" )
                } ).Single();

            var altNames = await Database.ExecuteQueryAsync( "SELECT Name FROM AlternativePersonName WHERE PersonId = " + id );
            person.AlternativeNames = altNames.SelectRows( row => row.GetString( "Name" ) ).ToArray();

            var roles = await Database.ExecuteQueryAsync(
                @"SELECT Production.Id AS ProdId, Production.Title, Production.ReleaseYear, Production.Genre, ProductionCast.CastRole, ProductionCharacter.Id AS CharId, ProductionCharacter.Name FROM
                  (ProductionCast LEFT JOIN ProductionCharacter ON ProductionCast.CharacterId = ProductionCharacter.Id)
                  JOIN Production ON ProductionCast.ProductionId = Production.Id
                  WHERE PersonId = " + id );
            person.Roles = roles
                .SelectRows( row => row )
                .ToLookup(
                row =>
                    new ProductionInfo(
                        row.GetInt( "ProdId" ),
                        row.GetString( "Title" ),
                        row.GetIntOpt( "ReleaseYear" ),
                        row.GetEnumOpt<ProductionGenre>( "Genre" ) ),
                row =>
                    new PersonRoleInfo(
                        row.GetEnum<PersonRole>( "CastRole" ),
                        row.HasValue( "CharId" ) ? new CharacterInfo( row.GetInt( "CharId" ), row.GetString( "Name" ) ) : null )
                );

            return person;
        }
    }
}