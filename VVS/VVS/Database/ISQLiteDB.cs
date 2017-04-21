using SQLite;

namespace VVS.Database
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
