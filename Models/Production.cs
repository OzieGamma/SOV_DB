namespace DB.Models
{
    public abstract class Production : IDatabaseModel
    {
        public int Id;
        public string Title;
        public int? Year;
        public ProductionGenre? Genre;

        public abstract void InsertIntoDb();
    }

    public sealed class VideoGame : Production
    {
        public override void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class Movie : Production
    {
        public MovieType Type;

        public override void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class Series : Production
    {
        public int? BeginningYear;
        public int? EndYear;

        public override void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class SeriesEpisode : Production
    {
        public long SeriesID;
        public int? SeasonNumber;
        public int? EpisodeNumber;

        public override void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}