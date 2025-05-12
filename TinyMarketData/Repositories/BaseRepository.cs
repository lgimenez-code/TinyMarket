using Microsoft.Data.SqlClient;

namespace TinyMarketData.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
