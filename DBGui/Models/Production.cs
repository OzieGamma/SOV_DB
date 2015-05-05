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

        public static async Task<Production> GetAsync( int id )
        {
            // First, get the type
            var table = await Database.ExecuteQueryAsync(
                @"SELECT VideoGame.ProductionId AS VideoGame, Movie.ProductionId AS Movie, Series.ProductionId AS Series, SeriesEpisode.ProductionId AS SeriesEpisode
                  FROM Production
                    JOIN VideoGame ON Production.Id = VideoGame.ProductionId
                    JOIN Movie ON Production.Id = Movie.ProductionId
                    JOIN Series ON Production.Id = Series.ProductionId
                    JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId
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
    }
}