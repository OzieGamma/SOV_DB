namespace DB
{
    using System;

    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }
    }
}
