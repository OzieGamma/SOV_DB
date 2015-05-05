﻿using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class VideoGame : Production
    {
        public static async Task<VideoGame> GetAsync( int id )
        {
            var table = await Database.ExecuteQueryAsync(
                @"SELECT Id, Title, ReleaseYear, Genre FROM
                  Production WHERE Id = " + id );
            return table.SelectRows( row =>
                new VideoGame
                {
                    Id = row.GetInt( "Id" ),
                    Title = row.GetString( "Title" ),
                    Year = row.GetIntOpt( "ReleaseYear" ),
                    Genre = row.GetEnumOpt<ProductionGenre>( "Genre" )
                } )
                .Single();
        }
    }
}