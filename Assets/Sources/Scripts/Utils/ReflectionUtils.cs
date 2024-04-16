using System;
using System.Linq;
using System.Reflection;
using Sources.Scripts.Annotation;
using UnityEngine;

namespace Sources.Scripts.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Inject by attr <see cref="JHandInject"/>
        /// </summary>
        public static void HandInject<T>(T target, object value) where T : class
        {
            // TODO refact!! field must get object for inject
            // TODO now oject find field for inject
            // TODO redundant iterations??

            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JHandInject))) continue;

                bool isTypeEqual = field.FieldType == value.GetType();
                bool isImplementInterface = value.GetType().GetInterfaces().Contains(field.FieldType);

                if (!isTypeEqual && !isImplementInterface) continue;

                field.SetValue(target, value);

                // for logging
                var val = value.GetType().ToString().Split('.').Last();
                var tar = target.ToString().Split('.').Last();
                JLog.Msg($"Injected. {val} to {tar}");
            }
        }
    }
}