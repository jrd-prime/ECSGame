using System.Reflection;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.TestConfig;

namespace ECSGame.Scripts.Core.DI
{
    // ReSharper disable once InconsistentNaming
    public static class InitializeDI
    {
        public static async UniTask Init(IMyContainer container)
        {
            container.Bind<IMyContainer>(container);
            await UniTask.Delay(1000);

            var bindsConfig = ConfigManager.Instance.GetConfiguration<IBindsConfiguration>();
            container.BindConfig(bindsConfig);
            await UniTask.Delay(1000);

            // 3. Inject services from cache to assembly by attr: [JInject]
            await container.InjectServicesAsync(Assembly.GetExecutingAssembly());
            await UniTask.Delay(1000);
        }
    }
}