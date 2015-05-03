using System;
using DB.Models;

namespace DBGui.Models
{
    public abstract class Production
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int? Year { get; private set; }
        public ProductionGenre? Genre { get; private set; }

        protected Production( int id, string title, int? year, ProductionGenre? genre )
        {
            Id = id;
            Title = title;
            Year = year;
            Genre = genre;
        }

        protected Production()
        {
            throw new NotSupportedException( "Just to compile." );
        }
    }
}