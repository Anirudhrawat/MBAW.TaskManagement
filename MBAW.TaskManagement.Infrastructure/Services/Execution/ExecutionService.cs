using MBAW.TaskManagement.Infrastructure.Services.SqlConnector;
using System.Data.SqlClient;
using System.Data;
using MBAW.TaskManagement.Infrastructure.Helpers;
using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Infrastructure.Services.Execution
{
    public class ExecutionService: IExecutionService
    {
        private readonly ISqlConnectorService _connectorService;
        
        public ExecutionService(ISqlConnectorService connectorService)
        {
            _connectorService = connectorService;
        }

        
        public async Task<List<T>> ExecuteStoredProcedureQueryAsync<T>(string storedProcedureName, Dictionary<string, object> parameters = null) where T : class, IBaseModel, new()
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException(Constants.ValidationMessages.EMPTY_STORED_PROCEDURE_NAME, nameof(storedProcedureName));

            using (var connection = _connectorService.GetConnection())
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.MapReaderToObjects<T>();
                }
            }
        }

        public async Task<int> ExecuteStoredProcedureNonQueryAsync(string storedProcedureName, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException(Constants.ValidationMessages.EMPTY_STORED_PROCEDURE_NAME, nameof(storedProcedureName));

            using (var connection = _connectorService.GetConnection())
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters if any
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                connection.Open();
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
