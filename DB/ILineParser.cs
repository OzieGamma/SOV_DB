namespace DB
{
    public interface ILineParser<out T>
    {
        string FileName { get; }
        T Parse( string[] values );
    }
}