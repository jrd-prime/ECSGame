using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.DI.Interface;
using UnityEngine;
using UnityEngine.Assertions;

namespace ECSGame.Scripts.Utils
{
    public class ReflectionUtils
    {
        private readonly IMyContainer _myContainer;

        public ReflectionUtils(IMyContainer myContainer)
        {
            _myContainer = myContainer;
        }

        /// <summary>
        /// Inject in private fields of object instance with creating new instances for inject
        /// </summary>
        /// <param name="targetInstance">Instance with field/s for inject with <see cref="JManualInject"/></param>
        /// <returns>Dictionary[Type, object] with type/instance injected</returns>
        public static async Task<Dictionary<Type, object>> ManualInjectAsync(object targetInstance)
        {
            if (targetInstance == null) throw new ArgumentNullException();

            var dict = new Dictionary<Type, object>();

            var fields = targetInstance.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JManualInject))) continue;

                var instance = Activator.CreateInstance(field.FieldType);

                dict.Add(field.FieldType, instance);

                field.SetValue(targetInstance, instance);
            }

            return await Task.FromResult(dict);
        }

        public static async Task ManualInjectWithInstanceAsync(object target, object instance)
        {
            if (target == null || instance == null) throw new ArgumentNullException();
            
            var fields = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JManualInject))) continue;

                bool isTypeEqual = field.FieldType == instance.GetType();
                bool isImplementInterface = instance.GetType().GetInterfaces().Contains(field.FieldType);

                if (!isTypeEqual && !isImplementInterface) continue;

                field.SetValue(target, instance);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Inject by attr <see cref="JManualInject"/>
        /// </summary>
        public ReflectionUtils HandInject<T>(T target, object instance) where T : class
        {
            // TODO refact!! field must get object for inject
            // TODO now object find field for inject
            // TODO redundant iterations??

            Assert.IsNotNull(target, "target != null");

            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JManualInject))) continue;

                bool isTypeEqual = field.FieldType == instance.GetType();
                bool isImplementInterface = instance.GetType().GetInterfaces().Contains(field.FieldType);

                if (!isTypeEqual && !isImplementInterface) continue;

                // for logging
                var val = Helper.TypeNameCutter(instance.GetType());

                field.SetValue(target, instance);
                // _myContainer.AddToCache(typeof(T), in instance);

                JLog.Msg($"Injected. {val} to {target}");
            }

            return this;
        }

        public static async Task ManualInjectInFieldWithoutAttr<T>(object target, T instance) where T : class
        {
            await Task.Run(() =>
            {
                Debug.LogWarning($"TARGET = " + target.GetType());
                FieldInfo[] fields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    Debug.LogWarning($"{field.FieldType} <- {typeof(T)}");

                    if (
                        typeof(T).IsAssignableFrom(field.FieldType) ||
                        field.FieldType == instance.GetType())
                    {
                        Debug.LogWarning($"{field.FieldType} assing with {typeof(T)}");
                        field.SetValue(target, instance);
                    }

                    // bool isTypeEqual = field.FieldType == instance.GetType();
                    // bool isImplementInterface = instance.GetType().GetInterfaces().Contains(field.FieldType);
                    //
                    // if (!isTypeEqual && !isImplementInterface) continue;
                }
            });
        }
    }
}