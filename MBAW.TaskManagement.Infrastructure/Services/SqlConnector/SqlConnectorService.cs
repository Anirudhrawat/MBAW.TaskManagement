using System.Data.SqlClient;

namespace MBAW.TaskManagement.Infrastructure.Services.SqlConnector
{
    public class SqlConnectorService : ISqlConnectorService
    {
        private readonly string _connectionString;

        public SqlConnectorService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
