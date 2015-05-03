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
    }
}