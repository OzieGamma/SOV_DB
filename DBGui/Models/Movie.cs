using DB.Models;

namespace DBGui.Models
{
    public sealed class Movie : Production
    {
        public MovieType Type { get; private set; }
    }
}