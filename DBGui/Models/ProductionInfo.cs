namespace DBGui.Models
{
    public sealed class ProductionInfo
    {
        public int Id { get; private set; }
        public string Title { get; private set; }

        public ProductionInfo( int id, string title )
        {
            Id = id;
            Title = title;
        }
    }
}