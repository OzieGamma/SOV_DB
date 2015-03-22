namespace DB.Models
{
    public abstract class Production
    {
        public int Id;
        public string Title;
        public int? Year;
    }

    public sealed class VideoGame : Production
    {
        public ProductionGenre? Genre;
    }

    public sealed class Movie : Production
    {
        public MovieType Type;
        public ProductionGenre? Genre;
    }

    public sealed class Series : Production
    {
        public int? BeginningYear;
        public int? EndYear;
        public ProductionGenre? Genre;
    }

    public sealed class SeriesEpisode : Production
    {
        public long SeriesID;
        public int? SeasonNumber;
        public int? EpisodeNumber;
    }
}