namespace DB
{
    public interface ILineParser<out T>
        where T : IDbModel
    {
        string FileName { get; }
        T Parse( string[] values );
    }
}