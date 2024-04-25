using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI.Interface;
using Sources.Scripts.Factory;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class DefaultContainerProvider : IContainerProvider
    {
        private const string ImplErrMsg = "The container configuration does not contain a specific " +
                                          "interface implementation type";

        private const string ConfigNullErrMsg = "The configuration is null!";

        public IMyContainer GetContainer()
        {
            IContainerConfig config = ConfigurationManager.I.GetConfiguration<IContainerConfig>();

            CheckConfig(ref config);

            var container = Activator.CreateInstance(config.Impl[typeof(IMyContainer)]);
            var binder = Activator.CreateInstance(config.Impl[typeof(IContainerBinder)]);
            var cache = Activator.CreateInstance(config.Impl[typeof(IContainerCache)]);
            var injector = Activator.CreateInstance(config.Impl[typeof(IContainerInjector)]);
            var serviceFactory = Activator.CreateInstance(config.Impl[typeof(IServiceFactory)]);

            ManualInject(container, binder);
            ManualInject(container, cache);
            ManualInject(container, injector);
            ManualInject(cache, serviceFactory);
            ManualInject(injector, container);
            ManualInject(injector, cache);


            Debug.LogWarning((container as IFieldsInjectable)?.IsFieldsInjected());
            Debug.LogWarning((cache as IFieldsInjectable)?.IsFieldsInjected());
            Debug.LogWarning((injector as IFieldsInjectable)?.IsFieldsInjected());

            return container as IMyContainer;
        }

        private static void ManualInject(object target, object obj)
        {
            var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                bool hasAttr = Attribute.IsDefined(field, typeof(JManualInject));
                bool isImplInterface = obj.GetType().GetInterfaces().Contains(field.FieldType);

                if (hasAttr && isImplInterface) field.SetValue(target, obj);
            }
        }

        private static void CheckConfig(ref IContainerConfig config)
        {
            if (config == null) throw new ArgumentNullException(ConfigNullErrMsg);

            Assert.True(
                config.Impl.ContainsKey(typeof(IMyContainer)),
                $"{ImplErrMsg} {typeof(IMyContainer)}");
            Assert.True(
                config.Impl.ContainsKey(typeof(IContainerBinder)),
                $"{ImplErrMsg} {typeof(IContainerBinder)}");
            Assert.True(
                config.Impl.ContainsKey(typeof(IContainerCache)),
                $"{ImplErrMsg} {typeof(IContainerCache)}");
            Assert.True(
                config.Impl.ContainsKey(typeof(IContainerInjector)),
                $"{ImplErrMsg} {typeof(IContainerInjector)}");
        }
    }
}