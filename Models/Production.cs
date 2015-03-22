using System.Collections.Generic;
using System.Threading.Tasks;

namespace DB.Models
{
    public abstract class Production : IDatabaseModel
    {
        public int Id;
        public string Title;
        public int? Year;
        public ProductionGenre? Genre;

        public virtual Task InsertInDatabaseAsync()
        {
            return Database.ExecuteNonQueryAsync(
                @"INSERT INTO Production (Id, Title, ReleaseYear, Genre)
                  VALUES (@Id, @Title, @ReleaseYear, @Genre);",
                new Dictionary<string, object>()
                {
                    { "@Id", Id },
                    { "@Title", Title },
                    { "@ReleaseYear", Year },
                    { "@Genre", Genre }
                }
            );
        }
    }

    public sealed class VideoGame : Production
    {
        public override async Task InsertInDatabaseAsync()
        {
            await base.InsertInDatabaseAsync();
            await Database.ExecuteNonQueryAsync(
                @"INSERT INTO VideoGame (ProductionId)
                  VALUES (@ProductionId);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", Id }
                }
            );
        }
    }

    public sealed class Movie : Production
    {
        public MovieType Type;

        public override async Task InsertInDatabaseAsync()
        {
            await base.InsertInDatabaseAsync();
            await Database.ExecuteNonQueryAsync(
                @"INSERT INTO Movie (ProductionId, MovieType)
                  VALUES (@ProductionId, @MovieType);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", Id },
                    { "@MovieType", Type }
                }
            );
        }
    }

    public sealed class Series : Production
    {
        public int? BeginningYear;
        public int? EndYear;

        public override async Task InsertInDatabaseAsync()
        {
            await base.InsertInDatabaseAsync();
            await Database.ExecuteNonQueryAsync(
                @"INSERT INTO Series (ProductionId, BeginningYear, EndYear)
                  VALUES (@ProductionId, @BeginningYear, @EndYear);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", Id },
                    { "@BeginningYear", BeginningYear },
                    { "@EndYear", EndYear }
                }
            );
        }
    }

    public sealed class SeriesEpisode : Production
    {
        public int SeriesId;
        public int? SeasonNumber;
        public int? EpisodeNumber;

        public override async Task InsertInDatabaseAsync()
        {
            await base.InsertInDatabaseAsync();
            await Database.ExecuteNonQueryAsync(
                @"INSERT INTO SeriesEpisode (ProductionId, SeriesId, SeasonNumber, EpisodeNumber)
                  VALUES (@ProductionId, @SeriesId, @SeasonNumber, @EpisodeNumber);",
                new Dictionary<string, object>()
                {
                    { "@ProductionId", Id },
                    { "@SeriesId", SeriesId },
                    { "@SeasonNumber", SeasonNumber },
                    { "@EpisodeNumber", EpisodeNumber }
                }
            );
        }
    }
}