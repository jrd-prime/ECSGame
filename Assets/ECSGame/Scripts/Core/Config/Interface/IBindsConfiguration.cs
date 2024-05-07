using System.Threading.Tasks;
using ECSGame.Scripts.Core.DI.Interface;

namespace ECSGame.Scripts.Core.Config.Interface
{
    /// <summary>
    /// Bind services 
    /// </summary>
    public interface IBindsConfiguration: IBindableConfiguration
    {
        public async Task InitBindings(IMyContainer container)
        {
            await Task.CompletedTask;
        }
    }
}