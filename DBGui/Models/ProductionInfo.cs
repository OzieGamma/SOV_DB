using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class ProductionInfo
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int? Year { get; private set; }
        public ProductionGenre? Genre { get; private set; }

        public ProductionInfo( int id, string title, int? year, ProductionGenre? genre )
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
        }

        public static Task<ProductionInfo[]> SearchByTitleAsync( string title )
        {
            return SearchAsync( string.Format( @"Title LIKE '%{0}%'", title ) );
        }

        public static async Task<ProductionInfo[]> SearchAsync( string condition )
        {
            var table = await Database.ExecuteQueryAsync( @"SELECT Id, Title, ReleaseYear, Genre FROM Production WHERE " + condition );
            return table.SelectRows( row =>
                new ProductionInfo(
                    row.GetInt( "Id" ),
                    row.GetString( "Title" ),
                    row.GetIntOpt( "ReleaseYear" ),
                    row.GetEnumOpt<ProductionGenre>( "Genre" )
                )
            ).ToArray();
        }
    }
}