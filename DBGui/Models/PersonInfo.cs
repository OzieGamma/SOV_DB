using System.Linq;
using System.Threading.Tasks;
using DB;
using DBGui.Utilities;

namespace DBGui.Models
{
    public sealed class PersonInfo
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public PersonInfo( int id, string firstName, string lastName )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public static Task<PersonInfo[]> SearchByNameAsync( string name )
        {
            return SearchAsync( string.Format(
@"FirstName LIKE '%{0}%' OR 
  LastName LIKE '%{0}%' OR
  (FirstName + ' ' + LastName) LIKE '%{0}%' OR
  (LastName + ' ' + FirstName) LIKE '%{0}%'",
                                            name ) );
        }

        public static async Task<PersonInfo[]> SearchAsync( string condition )
        {
            var table = await Database.ExecuteQueryAsync( @"SELECT Id, FirstName, LastName FROM Person WHERE " + condition );
            return table.SelectRows( row =>
                new PersonInfo(
                    row.GetInt( "Id" ),
                    row.GetString( "FirstName" ),
                    row.GetString( "LastName" )
                )
            ).ToArray();
        }

        public override bool Equals( object obj )
        {
            var info = obj as PersonInfo;
            return info != null && Id == info.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}