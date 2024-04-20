using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core;
using Sources.Scripts.TestConfig;
using UnityEngine;

namespace Sources.Scripts.DI
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public sealed class AppContext : MonoBehaviour
    {
        [JHandInject] private Container _container;
        [JHandInject] private IServiceConfig _serviceConfig;
        [JHandInject] private IBindsConfig _bindsConfig;

        private AppContext _context;

        public Container Container => _container;

        private void Awake() => DontDestroyOnLoad(this);
        
        public async Task InitializeAsync()
        {
            await _container.BindAsync(_bindsConfig as IBindableConfig);

            await Task.CompletedTask;

             // await _container.InitBinds(_bindsConfig);
            // await _container.InitServicesAsync(_serviceConfig.GetServicesList());
            // await _container.InjectDependenciesAsync(Assembly.GetExecutingAssembly());
        }
    }
}