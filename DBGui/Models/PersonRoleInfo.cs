using DB.Models;

namespace DBGui.Models
{
    public sealed class PersonRoleInfo
    {
        public PersonRole Role { get; private set; }
        public Character Character { get; private set; }

        public PersonRoleInfo( PersonRole role, Character character )
        {
            Role = role;
            Character = character;
        }
    }
}