namespace DB
{
    public interface IOutput
    {
        void WriteLine(string text, params object[] args);
    }
}
