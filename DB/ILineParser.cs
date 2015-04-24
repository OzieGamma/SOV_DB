using System.Collections.Generic;

namespace DB
{
    public interface ILineParser
    {
        string FileName { get; }
        IEnumerable<object> Parse( string[] values );
    }
}