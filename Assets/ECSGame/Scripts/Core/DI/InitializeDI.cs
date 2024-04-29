using System.Reflection;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestConfig;
using ECSGame.Scripts.Utils;

namespace ECSGame.Scripts.Core.DI
{
    // ReSharper disable once InconsistentNaming
    public class InitializeDI : ILoadable
    {
        private IMyContainer _container;

        public async UniTask Load()
        {
            JLog.Msg("\t(!) Initialization started!");
            _container = AppContext.Instance.ContainerProvider.GetContainer();

            await InitAndBindContainer();
            await BindServicesFromConfigs();
            await InjectServicesFromCacheToAssembly();
            JLog.Msg("\t(!) Initialization FINISHED!");
        }


        private async UniTask InitAndBindContainer()
        {
            _container.Bind<IMyContainer>(_container);
            await UniTask.Delay(1000);
            JLog.Msg("\t(!) Container initialization complete!");
        }

        private async UniTask BindServicesFromConfigs()
        {
            var bindsConfig = AppContext.Instance.ConfigManager.GetConfiguration<IBindsConfiguration>();
            _container.BindConfig(bindsConfig);
            await UniTask.Delay(1000);
            JLog.Msg("\t(!) Bind services from config complete!");
        }

        private async UniTask InjectServicesFromCacheToAssembly()
        {
            // 3. Inject services from cache to assembly by attr: [JInject]
            await _container.InjectServicesAsync(Assembly.GetExecutingAssembly());
            await UniTask.Delay(1000);
            JLog.Msg("\t(!) Dependency injection complete!");
        }
    }
}