using System;
using System.Linq;
using System.Reflection;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Sources.Scripts.Utils
{
    public class ReflectionUtils
    {
        private Container _container;

        public ReflectionUtils(Container container)
        {
            _container = container;
        }

        public void AsSingle()
        {
        }

        /// <summary>
        /// Inject by attr <see cref="JHandInject"/>
        /// </summary>
        public ReflectionUtils HandInject<T>(T target, object instance) where T : class
        {
            // TODO refact!! field must get object for inject
            // TODO now object find field for inject
            // TODO redundant iterations??

            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JHandInject))) continue;

                bool isTypeEqual = field.FieldType == instance.GetType();
                bool isImplementInterface = instance.GetType().GetInterfaces().Contains(field.FieldType);

                if (!isTypeEqual && !isImplementInterface) continue;

                field.SetValue(target, instance);
                _container.AddToCache(typeof(T), instance);

                // for logging
                var val = Helper.TypeNameCutter(instance.GetType());
                var tar = Helper.TypeNameCutter(target.GetType());
                JLog.Msg($"Injected. {val} to {tar}");
            }

            return this;
        }
    }
}