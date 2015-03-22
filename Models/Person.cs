using System;
using System.Collections.Generic;

namespace DB.Models
{
    public sealed class Person : IDatabaseModel
    {
        public int Id;
        public string Name;
        public Gender? Gender;
        public string Trivia;
        public string Quotes;
        public DateTimeOffset? BirthDate;
        public DateTimeOffset? DeathDate;
        public string BirthName;
        public string ShortBio;
        public PersonSpouseInfo Spouse;
        public decimal? Height;

        public async void InsertIntoDb()
        {
            if ( Spouse != null )
            {
                // We do something a bit weird, and keep the person ID for the spouse ID, since in the date there is only one.
                // Since Person.SpouseId is a foreign key, we need to insert the spouse first
                await DatabaseConnection.ExecuteNonQueryAsync(
                    @"INSERT INTO PersonSpouse(Id, Name, IsInDatabase, BeginDate, EndDate, EndNotes, ChildrenCount, ChildrenDescription)
                      VALUES (@Id, @Name, @IsInDatabase, @BeginDate, @EndDate, @EndNotes, @ChildrenCount, @ChildrenDescription);",
                    new Dictionary<string, object>
                    {
                        { "@Id", Id },
                        { "@Name", Spouse.Name },
                        { "@IsInDatabase", Spouse.IsInDatabase },
                        { "@BeginDate", Spouse.BeginDate },
                        { "@EndDate", Spouse.EndDate },
                        { "@EndNotes", Spouse.EndNotes },
                        { "@ChildrenCount", Spouse.ChildrenCount },
                        { "@ChildrenDescription", Spouse.ChildrenDescription }
                    }
                );
            }

            await DatabaseConnection.ExecuteNonQueryAsync(
                @"INSERT INTO Person(Id, Name, Gender, Trivia, Quotes, BirthDate, DeathDate, BirthName, ShortBio, Height, SpouseId)
                  VALUES (@Id, @Name, @Gender, @Trivia, @Quotes, @BirthDate, @DeathDate, @BirthName, @ShortBio, @Height, @SpouseId);",
                new Dictionary<string, object>
                {
                    { "@Id", Id },
                    { "@Name", Name },
                    { "@Gender", Gender == null ? null : Gender == Models.Gender.Male ? "M" : "F" },
                    { "@Trivia", Trivia },
                    { "@Quotes", Quotes },
                    { "@BirthDate", BirthDate },
                    { "@DeathDate", DeathDate },
                    { "@BirthName", BirthName },
                    { "@ShortBio", ShortBio },
                    { "@Height", Height },
                    { "@SpouseId", Spouse == null ? (int?) null : Id }
                }
            );
        }
    }
}
