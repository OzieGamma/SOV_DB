using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DB.Models;

namespace DBGui.Models
{
    public sealed class Person
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Gender? Gender { get; private set; }
        public string Trivia { get; private set; }
        public string Quotes { get; private set; }
        public DateTimeOffset? BirthDate { get; private set; }
        public DateTimeOffset? DeathDate { get; private set; }
        public string BirthName { get; private set; }
        public string ShortBio { get; private set; }
        public string SpouseInfo { get; private set; }
        public int? Height { get; private set; }

        public string[] AlternativeNames { get; private set; }
        public Dictionary<ProductionInfo, PersonRoleInfo> Roles { get; private set; }

        public static Task<Person> GetAsync( int id )
        {
            throw new NotImplementedException();
        }
    }
}