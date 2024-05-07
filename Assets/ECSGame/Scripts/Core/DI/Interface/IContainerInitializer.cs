using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IContainerInitializer : ILoadable
    {
        string ILoadable.Description => "Initialize container";

        public IMyContainer GetContainer();
    }
}