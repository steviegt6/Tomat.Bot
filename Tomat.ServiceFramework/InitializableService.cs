using System;
using System.Threading.Tasks;
using Tomat.ServiceFramework.Interfaces;

namespace Tomat.ServiceFramework
{
    public abstract class InitializableService : ServiceBase, IInitializable
    {
        protected InitializableService(IServiceProvider services) : base(services)
        {
        }

        public virtual async Task InitializeAsync() => await Task.CompletedTask;
    }
}