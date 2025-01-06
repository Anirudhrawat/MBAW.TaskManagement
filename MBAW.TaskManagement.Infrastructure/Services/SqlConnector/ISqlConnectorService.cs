using System.Data.SqlClient;

namespace MBAW.TaskManagement.Infrastructure.Services.SqlConnector
{
    public interface ISqlConnectorService
    {
        public SqlConnection GetConnection();
    }
}
