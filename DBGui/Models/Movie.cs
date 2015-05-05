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

        public static async Task<Movie> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT Id, Title, ReleaseYear, Genre, Type FROM
                  Production JOIN Movie ON Production.Id = Movie.ProductionId
                  WHERE Id = " + id );
            return table.SelectRows( row =>
                new Movie
                {
                    Id = row.GetInt( "Id" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" ),
                    Type = row.GetEnum<MovieType>( "Type" )
                } )
                .Single();
        }
    }
}