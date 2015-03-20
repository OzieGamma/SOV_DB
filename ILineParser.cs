namespace DB
{
    public interface ILineParser<out T>
        where T : class
    {
        string FileName { get; }
        T Parse( string[] values );
    }
}