using MBAW.TaskManagement.Infrastructure.Model;
using MBAW.TaskManagement.Infrastructure.Services.Execution;
using MBAW.TaskManagement.Infrastructure.Services.Seeding;
using MBAW.TaskManagement.Infrastructure.Services.SqlConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MBAW.TaskManagement.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString(Constants.Misc.CONNECTION_STRING_KEY);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(Constants.ValidationMessages.EMPTY_CONNECTION_STRING);
            }
            services.AddTransient<ISqlConnectorService>(provider => new SqlConnectorService(connectionString));
            services.AddTransient<ISeedingService, SeedingService>();
            services.AddTransient<IExecutionService, ExecutionService>();
        }

        public static void AddStoredProcedureRegistry<T>(this IServiceCollection services) where T: class, IStoredProcedureRegistry
        {
            services.AddSingleton<IStoredProcedureRegistry, T>();
        }

        public static void StartSeeding(this IServiceProvider serviceProvider, Type registryType)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var seedingService = scope.ServiceProvider.GetRequiredService<ISeedingService>();
                Assembly domainAssembly = Assembly.GetAssembly(registryType) ?? throw new ArgumentNullException(nameof(domainAssembly));
                seedingService.StartSeeding(domainAssembly);
            }
        }
    }
}
