using System;

namespace DB
{
    public static class Program
    {
        // All CSV files must be in that folder, without renaming them
        private const string InputFilesPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
        @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        private static void Main()
        {
            Console.WriteLine( "Doing work..." );
            DatabaseImport.ImportFromDirectoryAsync( InputFilesPath ).Wait();
            Console.Read();
        }
    }
}