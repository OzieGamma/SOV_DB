using System;

namespace DB.Models
{
    public sealed class Person
    {
        public long Id;
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
    }
}