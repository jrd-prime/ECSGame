using System.Threading.Tasks;

namespace Sources.Scripts
{
    public interface IAppContext
    {
        public Task InitializeAsync();
    }
}