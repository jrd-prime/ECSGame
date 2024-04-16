using System;
using System.Linq;
using System.Reflection;
using Sources.Scripts.Annotation;

namespace Sources.Scripts.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Inject by attr <see cref="JHandInject"/>
        /// </summary>
        public static void HandInject<T>(T target, object value) where T : class
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                bool hasAttr = Attribute.IsDefined(field, typeof(JHandInject));
                bool isTypeEqual = field.FieldType == value.GetType();

                if (!hasAttr || !isTypeEqual) continue;

                field.SetValue(target, value);

                // for logging
                var val = value.GetType().ToString().Split('.').Last();
                var tar = target.ToString().Split('.').Last();
                JLogger.Msg($"Injected. {val} to {tar}");
            }
        }
    }
}