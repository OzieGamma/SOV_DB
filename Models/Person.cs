namespace DB.Models
{
    using System;
    using System.Collections.Generic;

    public sealed class Person : IDbModel
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

        public async void InsertIntoDb()
        {
            var parameters = new Dictionary<string, object>();
            parameters["@Id"] = this.Id;
            parameters["@Name"] = this.Name;
            parameters["@Gender"] = this.Gender == Models.Gender.Male ? "M" : "F";
            parameters["@Trivia"] = this.Trivia;
            parameters["@Quotes"] = this.Quotes;
            parameters["@BirthDate"] = this.BirthDate;
            parameters["@DeathDate"] = this.DeathDate;
            parameters["@BirthName"] = this.BirthName;
            parameters["@ShortBio"] = this.ShortBio;

            if ( this.Spouse != null )
            {
                parameters["@SpouseName"] = this.Spouse.Name ?? "NULL";
                parameters["@SpouseIsInDatabase"] = this.Spouse.IsInDatabase;
                parameters["@SpouseBeginDate"] = this.Spouse.BeginDate;
                parameters["@SpouseEndDate"] = this.Spouse.EndDate;
                parameters["@SpouseEndNotes"] = this.Spouse.EndNotes;
                parameters["@SpouseChildrenCount"] = this.Spouse.ChildrenCount;
                parameters["@SpouseChildrenDescription"] = this.Spouse.ChildrenDescription;
            }
            else
            {
                parameters["@SpouseName"] = null;
                parameters["@SpouseIsInDatabase"] = null;
                parameters["@SpouseBeginDate"] = null;
                parameters["@SpouseEndDate"] = null;
                parameters["@SpouseEndNotes"] = null;
                parameters["@SpouseChildrenCount"] = null;
                parameters["@SpouseChildrenDescription"] = null;
            }
            parameters["@Height"] = this.Height;

            int rowsInserted =
                await
                DBConnection.ExecuteNonQuery(
                    @"INSERT INTO dbo.Person VALUES (@Id, @Name, @Gender, @Trivia, @Quotes, @BirthDate, @DeathDate, @BirthName, @ShortBio, @SpouseName, @SpouseIsInDatabase, @SpouseBeginDate, @SpouseEndDate, @SpouseEndNotes, @SpouseChildrenCount, @SpouseChildrenDescription, @Height);",
                    parameters );

            if ( rowsInserted == 0 )
            {
                throw new Exception( "Could not insert" );
            }
        }
    }
}