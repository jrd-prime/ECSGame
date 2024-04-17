using System;
using Sources.Scripts.DI;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;
using AppContext = Sources.Scripts.DI.AppContext;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;

        private void Awake()
        {
            DontDestroyOnLoad(_appContextHolderGo);
        }

        private async void Start()
        {
            #region PreInit

            Container container = new();
            IBindsConfig bindsConfig = new TestBinds();

            #endregion

            #region ImportantBindings

            await container.BindSelfAsync<Container>();

            AppContext context = await container.BindSelfAsync<AppContext>(_appContextHolderGo);
            ServiceFactory serviceFactory = await container.BindSelfAsync<ServiceFactory>();

            #endregion

            #region Bindings

            await bindsConfig.InitBindings(container);

            #endregion

            #region Injection

            // Mini app start hand inject
            var reflectionUtils = new ReflectionUtils(container);
            
            reflectionUtils.HandInject(context, bindsConfig);
            reflectionUtils.HandInject(context, container);
            reflectionUtils.HandInject(container, serviceFactory);

            await context.InitializeAsync();

            #endregion

// TODO on app start and fake loading services - create loading screen

// TODO add asset loadin async init binds

            Debug.LogWarning($"in cache binds: {container.getBinds().Count}".ToUpper());
            Debug.LogWarning($"in cache instances: {container.GetCache().Count}".ToUpper());
            
            Debug.LogWarning("INITIALISATION FINISHEEEDDDD!!!!");
            Debug.LogWarning("STAAAART THHHE GAMEE");
        }
    }
}