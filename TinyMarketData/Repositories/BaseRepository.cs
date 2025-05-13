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

        /// <summary>
        /// crea la conexion a la base de datos
        /// </summary>
        /// <returns></returns>
        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
