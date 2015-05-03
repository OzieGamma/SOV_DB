using System;
using DB.Models;

namespace DB.Internals.ImportModels
{
    internal sealed class Person
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
        public int? Height;

        public override string ToString()
        {
            return string.Join( "\t", Id, FirstName, LastName, Gender, Trivia, Quotes, BirthDate.HasValue ? BirthDate.Value.ToString( "O" ) : "", DeathDate.HasValue ? DeathDate.Value.ToString( "O" ) : "", BirthName, ShortBio, SpouseInfo, Height );
        }
    }
}
