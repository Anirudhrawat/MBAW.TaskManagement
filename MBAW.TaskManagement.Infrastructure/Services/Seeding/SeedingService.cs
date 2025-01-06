using MBAW.TaskManagement.Infrastructure.Model;
using MBAW.TaskManagement.Infrastructure.Services.SqlConnector;
using System.Data.SqlClient;
using System.Reflection;

namespace MBAW.TaskManagement.Infrastructure.Services.Seeding
{
    public class SeedingService: ISeedingService
    {
        private readonly ISqlConnectorService _connectorService;
        private readonly IStoredProcedureRegistry _registry;

        public SeedingService(ISqlConnectorService connectorService, IStoredProcedureRegistry registry)
        {
            _connectorService = connectorService;
            _registry = registry;
        }

        public void StartSeeding(Assembly registryAssembly)
        {
            if (registryAssembly == null)
                throw new ArgumentNullException(nameof(registryAssembly));
            string assemblyLocation = registryAssembly.Location;
            if (string.IsNullOrEmpty(assemblyLocation))
                throw new InvalidOperationException(Constants.ValidationMessages.INVALID_REGISTRY_LOCATION);

            string storedProceduresFolder = Path.Combine(
                Path.GetDirectoryName(assemblyLocation) ?? string.Empty,
                Constants.Misc.STORED_PROCEDURE_DIRECTORY);

            if (!Directory.Exists(storedProceduresFolder))
                throw new DirectoryNotFoundException(string.Format(Constants.ValidationMessages.NO_REGISTRY_DIRECTORY, storedProceduresFolder));

            var registeredStoredProcedures = _registry.GetRegisterStoredProcedures();
            if (registeredStoredProcedures.Any())
            {
                var sqlConnection = _connectorService.GetConnection();
                sqlConnection.Open();
                try
                {
                    foreach (var storedProcedureName in registeredStoredProcedures)
                    {
                        string filePath = Path.Combine(storedProceduresFolder, $"{storedProcedureName}.sql");

                        if (!File.Exists(filePath))
                        {
                            throw new Exception(Constants.ValidationMessages.STORED_PROCEDURE_NOT_FOUND);
                        }
                        string storedProcedureScript = File.ReadAllText(filePath);
                        using var command = new SqlCommand(storedProcedureScript, sqlConnection);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

    }
}
