using MBAW.TaskManagement.Infrastructure.Model;

namespace MBAW.TaskManagement.Infrastructure.Services.Execution
{
    public interface IExecutionService
    {
        public Task<List<T>> ExecuteStoredProcedureQueryAsync<T>(string storedProcedureName, Dictionary<string, object> parameters = null) where T : class, IBaseModel, new();
        public Task<int> ExecuteStoredProcedureNonQueryAsync(string storedProcedureName, Dictionary<string, object> parameters = null);
    }
}
