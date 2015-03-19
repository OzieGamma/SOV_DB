namespace DB
{
    public interface LineParser<out T>
        where T : class
    {
        string FileName { get; }
        T Parse( string[] values );
    }
}