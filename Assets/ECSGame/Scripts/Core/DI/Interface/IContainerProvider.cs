using Cysharp.Threading.Tasks;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IContainerProvider : ILoadable
    {
        public IMyContainer GetContainer();
    }
}