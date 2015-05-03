using DB.Models;

namespace DBGui.Models
{
    public sealed class PersonRoleInfo
    {
        public PersonRole Role { get; private set; }
        public CharacterInfo Character { get; private set; }

        public PersonRoleInfo( PersonRole role, CharacterInfo character )
        {
            Role = role;
            Character = character;
        }
    }
}