using System.Threading.Tasks;

namespace Sources.Scripts.DI.Interface
{
    public interface IAppContext
    {
        public Task InitializeAsync();
    }
}