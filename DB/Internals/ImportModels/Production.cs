using DB.Models;
namespace DB.Internals.ImportModels
{
    internal sealed class Production
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

    internal sealed class VideoGame
    {
        public int ProductionId;

        public override string ToString()
        {
            return ProductionId.ToString();
        }
    }

    internal sealed class Movie
    {
        public int ProductionId;
        public MovieType Type;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, Type );
        }
    }

    internal sealed class Series
    {
        public int ProductionId;
        public int? BeginningYear;
        public int? EndYear;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, BeginningYear, EndYear );
        }
    }

    internal sealed class SeriesEpisode
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