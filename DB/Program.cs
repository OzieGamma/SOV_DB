namespace DB
{
    using System;

    public static class Program
    {
        // All CSV files must be in that folder, without renaming them
        private const string InputFilesPath =
            //@"C:\Users\Oswald\Downloads\Movies"; // Oswald
            @"X:\Documents\EPFL\DB\Project dataset"; // Solal

        public static void Main()
        {
            Console.WriteLine("Doing work...");
            new Import(new ConsoleOutput()).FromCsvDirectory(InputFilesPath).Wait();
            Console.Read();
        }
    }
}
