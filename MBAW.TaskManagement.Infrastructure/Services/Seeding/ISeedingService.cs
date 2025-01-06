using System.Reflection;

namespace MBAW.TaskManagement.Infrastructure.Services.Seeding
{
    public interface ISeedingService
    {
        void StartSeeding(Assembly registryAssembly);
    }
}
