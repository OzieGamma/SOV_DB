using System;

namespace DB.Models
{
    public sealed class Person
    {
        public int Id;
        public string FirstName;
        public string LastName;
        public Gender? Gender;
        public string Trivia;
        public string Quotes;
        public DateTimeOffset? BirthDate;
        public DateTimeOffset? DeathDate;
        public string BirthName;
        public string ShortBio;
        public string SpouseInfo;
        public decimal? Height;

        public override string ToString()
        {
            return string.Join( "\t", Id, FirstName, LastName, Gender, Trivia, Quotes, BirthDate, DeathDate, BirthName, ShortBio, SpouseInfo, Height );
        }
    }
}
