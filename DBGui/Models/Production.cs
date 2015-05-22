using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public abstract class Production
    {
        public int Id { get; protected set; }
        public string Title { get; protected set; }
        public int? Year { get; protected set; }
        public ProductionGenre? Genre { get; protected set; }

        public ILookup<PersonInfo, PersonRoleInfo> People { get; protected set; }

        public static async Task<Production> GetAsync( int id )
        {
            // First, get the type
            var table = await Database.ExecuteQueryAsync(
                @"SELECT VideoGame.ProductionId AS VideoGame, Movie.ProductionId AS Movie, Series.ProductionId AS Series, SeriesEpisode.ProductionId AS SeriesEpisode
                  FROM Production
                    LEFT JOIN VideoGame ON Production.Id = VideoGame.ProductionId
                    LEFT JOIN Movie ON Production.Id = Movie.ProductionId
                    LEFT JOIN Series ON Production.Id = Series.ProductionId
                    LEFT JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId
                  WHERE Production.Id = " + id );
            var row = table.SelectRows( r => r ).Single();

            if ( row.HasValue( "VideoGame" ) )
            {
                return await VideoGame.GetAsync( id );
            }

            if ( row.HasValue( "Movie" ) )
            {
                return await Movie.GetAsync( id );
            }

            if ( row.HasValue( "Series" ) )
            {
                return await Series.GetAsync( id );
            }

            return await Episode.GetAsync( id );
        }

        protected static async Task<ILookup<PersonInfo, PersonRoleInfo>> GetCharactersAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT PersonId, Person.LastName, Person.FirstName, CastRole, CharacterId, ProductionCharacter.Name
                  FROM ProductionCast
                    LEFT JOIN ProductionCharacter ON ProductionCast.CharacterId = ProductionCharacter.Id
                    LEFT JOIN Person ON ProductionCast.PersonId = Person.Id
                  WHERE ProductionId = " + id );
            return table.SelectRows( r => r )
                        .ToLookup(
                            row => new PersonInfo( row.GetInt( "PersonId" ), row.GetString( "FirstName" ), row.GetString( "LastName" ) ),
                            row => new PersonRoleInfo( row.GetEnum<PersonRole>( "CastRole" ), row.HasValue( "CharacterId" ) ? new CharacterInfo( row.GetInt( "CharacterId" ), row.GetString( "Name" ) ) : null )
                        );
        }
    }
}