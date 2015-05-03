namespace DBGui.Models
{
    public sealed class Episode : Production
    {
        public Series Series { get; internal set; }
        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
    }
}