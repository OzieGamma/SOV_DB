using System.Collections.Generic;

namespace DB.Internals.Parsing
{
    internal interface ILineParser
    {
        string FileName { get; }
        IEnumerable<object> Parse( string[] values );
    }
}