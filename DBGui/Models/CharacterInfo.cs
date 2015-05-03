using System.Linq;
using System.Threading.Tasks;
using DB;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class CharacterInfo
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CharacterInfo( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public static Task<CharacterInfo[]> SearchByNameAsync( string name )
        {
            return SearchAsync( string.Format( @"Name LIKE '%{0}%'", name ) );
        }

        public static async Task<CharacterInfo[]> SearchAsync( string condition )
        {
            var table = await Database.ExecuteQueryAsync( @"SELECT Id, Name FROM ProductionCharacter WHERE " + condition );
            return table.SelectRows( row =>
                new CharacterInfo(
                    row.GetInt( "Id" ),
                    row.GetString( "Name" )
                )
            ).ToArray();
        }

        public override bool Equals( object obj )
        {
            var info = obj as CharacterInfo;
            return info != null && Id == info.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}