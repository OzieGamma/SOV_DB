using System.Threading.Tasks;

namespace DB
{
    public interface IDatabaseModel
    {
        Task InsertInDatabaseAsync();
    }
}