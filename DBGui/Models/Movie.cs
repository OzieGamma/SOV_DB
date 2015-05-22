using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class Movie : Production
    {
        public MovieType Type { get; private set; }

        public new static async Task<Movie> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT ProductionId, Title, ReleaseYear, Genre, MovieType FROM
                  Production JOIN Movie ON Production.Id = Movie.ProductionId
                  WHERE Id = " + id );
            var movie = table.SelectRows( row =>
                new Movie
                {
                    Id = row.GetInt( "ProductionId" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" ),
                    Type = row.GetEnum<MovieType>( "MovieType" ),
                } )
                .Single();
            movie.People = await GetCharactersAsync( id );
            return movie;
        }
    }
}