using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Tomat.ServiceFramework
{
    public static class ServiceRegistry
    {
        public static async Task InitializeServicesAsync(IServiceCollection collection, ServiceProvider provider)
        {
            foreach (ServiceDescriptor descriptor in collection!)
            foreach (object? service in provider.GetServices(descriptor.ServiceType)
                .Where(x => x != null && x.GetType().IsSubclassOf(typeof(ServiceBase))))
            {
                if (service is not ServiceBase sBase) 
                    return;

                if (sBase is InitializableService initService)
                    await initService.InitializeAsync();
            }
        }
    }
}
