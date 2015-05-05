﻿using System.Linq;
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

        public static async Task<Series> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT Id, Title, ReleaseYear, Genre, BeginningYear, EndYear FROM
                  Production JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId
                  WHERE Id = " + id );
            var series = table.SelectRows( row =>
                new Series
                {
                    Id = row.GetInt( "Id" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" ),
                    BeginningYear = row.GetIntOpt( "BeginningYear" ),
                    EndYear = row.GetIntOpt( "EndYear" )
                } )
                .Single();

            var episodesInfo = await Database.ExecuteQueryAsync( @"SELECT Id, Title, ReleaseYear, Genre FROM 
Production JOIN SeriesEpisode ON Production.Id = SeriesEpisode.ProductionId WHERE SeriesId = " + id );
            series.Episodes = episodesInfo.SelectRows(
                row => new ProductionInfo(
                        row.GetInt( "ProdId" ),
                        row.GetString( "Title" ),
                        row.GetIntOpt( "ReleaseYear" ),
                        row.GetEnumOpt<ProductionGenre>( "Genre" ) ) ).ToArray();

            return series;
        }
    }
}