namespace DB.Models
{
    public sealed class Production
    {
        public int Id;
        public string Title;
        public int? Year;
        public ProductionGenre? Genre;

        public override string ToString()
        {
            return string.Join( "\t", Id, Title, Year, Genre );
        }
    }

    public sealed class VideoGame
    {
        public int ProductionId;

        public override string ToString()
        {
            return ProductionId.ToString();
        }
    }

    public sealed class Movie
    {
        public int ProductionId;
        public MovieType Type;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, Type );
        }
    }

    public sealed class Series
    {
        public int ProductionId;
        public int? BeginningYear;
        public int? EndYear;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, BeginningYear, EndYear );
        }
    }

    public sealed class SeriesEpisode
    {
        public int ProductionId;
        public int SeriesId;
        public int? SeasonNumber;
        public int? EpisodeNumber;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, SeriesId, SeasonNumber, EpisodeNumber );
        }
    }
}