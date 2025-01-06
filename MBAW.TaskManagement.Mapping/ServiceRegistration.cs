using MBAW.TaskManagement.Mapping.Mappings;

using Microsoft.Extensions.DependencyInjection;

namespace MBAW.TaskManagement.Mapping
{
    public static class ServiceRegistration
    {
        public static void AddMappingLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TaskProfile));
        }
    }
}
