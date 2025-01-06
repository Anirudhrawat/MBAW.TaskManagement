using MBAW.TaskManagement.Application.Services.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBAW.TaskManagement.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITaskService, TaskService>();
        }
    }
}
