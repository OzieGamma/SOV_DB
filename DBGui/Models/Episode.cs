using System;
using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class Episode : Production
    {
        public ProductionInfo Series { get; private set; }
        public int? SeasonNumber { get; private set; }
        public int? EpisodeNumber { get; private set; }

        public new static async Task<Episode> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT ProductionId, Title, ReleaseYear, Genre, SeriesId, SeasonNumber, EpisodeNumber FROM
                  Production JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId
                  WHERE Id = " + id );
            var episode = table.SelectRows( row =>
                Tuple.Create( new Episode
                {
                    Id = row.GetInt( "ProductionId" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" ),
                    SeasonNumber = row.GetIntOpt( "SeasonNumber" ),
                    EpisodeNumber = row.GetIntOpt( "EpisodeNumber" )
                }, row.GetInt( "SeriesId" ) ) )
                .Single();

            var seriesInfo = await Database.ExecuteQueryAsync( "SELECT Id, Title, ReleaseYear, Genre FROM Production WHERE Id = " + episode.Item2 );
            episode.Item1.Series = seriesInfo.SelectRows(
                row => new ProductionInfo(
                        row.GetInt( "Id" ),
                        row.GetString( "Title" ),
                        row.GetIntOpt( "ReleaseYear" ),
                        row.GetEnumOpt<ProductionGenre>( "Genre" ) ) ).Single();

            return episode.Item1;
        }
    }
}