using System.Threading.Tasks;

namespace Tomat.ServiceFramework.Interfaces
{
    public interface IInitializable
    {
        public Task InitializeAsync(); // should be async smh
    }
}