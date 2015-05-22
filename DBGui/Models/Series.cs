using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class Series : Production
    {
        public int? BeginningYear { get; private set; }
        public int? EndYear { get; private set; }
        public ProductionInfo[] Episodes { get; private set; }

        public new static async Task<Series> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT ProductionId, Title, ReleaseYear, Genre, BeginningYear, EndYear FROM
                  Production JOIN Series ON Production.Id = Series.ProductionId
                  WHERE Id = " + id );
            var series = table.SelectRows( row =>
                new Series
                {
                    Id = row.GetInt( "ProductionId" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" ),
                    BeginningYear = row.GetIntOpt( "BeginningYear" ),
                    EndYear = row.GetIntOpt( "EndYear" )
                } )
                .Single();

            var episodesInfo = await Database.ExecuteQueryAsync( @"SELECT ProductionId, Title, ReleaseYear, Genre FROM 
Production JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId WHERE SeriesId = " + id );
            series.Episodes = episodesInfo.SelectRows(
                row => new ProductionInfo(
                        row.GetInt( "ProductionId" ),
                        row.GetString( "Title" ),
                        row.GetIntOpt( "ReleaseYear" ),
                        row.GetEnumOpt<ProductionGenre>( "Genre" ) ) ).ToArray();

            return series;
        }
    }
}