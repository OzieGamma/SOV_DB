namespace DBGui
{
    using System;

    using DB;

    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }
    }
}
