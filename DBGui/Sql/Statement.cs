namespace DBGui.Sql
{
    public sealed class Statement
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Command { get; private set; }

        public Statement( string name, string description, string command )
        {
            Name = name;
            Description = description;
            Command = command;
        }
    }
}