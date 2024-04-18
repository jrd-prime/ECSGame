using System;
using System.Linq;
using System.Reflection;
using Sources.Scripts.Annotation;
using Sources.Scripts.DI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Sources.Scripts.Utils
{
    public class ReflectionUtils
    {
        private Container _container;

        public ReflectionUtils(Container container)
        {
            _container = container;
        }

        /// <summary>
        /// Inject by attr <see cref="JHandInject"/>
        /// </summary>
        public ReflectionUtils HandInject<T>(T target, object instance) where T : class
        {
            // TODO refact!! field must get object for inject
            // TODO now object find field for inject
            // TODO redundant iterations??

            Assert.IsNotNull(target, "target != null");


            FieldInfo[] fields =
                typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JHandInject))) continue;

                bool isTypeEqual = field.FieldType == instance.GetType();
                bool isImplementInterface = instance.GetType().GetInterfaces().Contains(field.FieldType);

                if (!isTypeEqual && !isImplementInterface) continue;

                Debug.LogWarning(target);

                // for logging
                var val = Helper.TypeNameCutter(instance.GetType());

                field.SetValue(target, instance);
                _container.Cache.Add(typeof(T), instance);

                JLog.Msg($"Injected. {val} to {target}");
            }

            return this;
        }
    }
}