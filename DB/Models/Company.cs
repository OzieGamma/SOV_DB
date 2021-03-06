﻿namespace DB.Models
{
    public sealed class Company
    {
        public int Id;
        public string CountryCode;
        public string Name;

        public override string ToString()
        {
            return string.Join( "\t", Id, CountryCode, Name );
        }
    }
}