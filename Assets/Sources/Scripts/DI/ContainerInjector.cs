using System;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class ContainerInjector
    {
        [JManualInject] private ContainerCache _cache;
        [JManualInject] private MyContainer _container;

        public async Task InjectDependenciesAsync(Assembly assembly)
        {
            Debug.LogWarning("ASS ===== ");
            foreach (var type in assembly.GetTypes())
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    // JLog.Msg($"= = = = = =");
                    // JLog.Msg($"type = {type}");
                    // JLog.Msg($"field = {field}");
                    if (!Attribute.IsDefined(field, typeof(JInject))) continue;

                    // JLog.Msg($"field has attr for inject");

                    var target = field.FieldType;
                    // JLog.Msg($"target (field type) = {target}");

                    Debug.LogWarning("count cache = " + _cache.GetCache().Count);

                    var instance = _cache.GetCache()[target];

                    JLog.Msg($"instance from cache = {instance}");

                    var serviceInstanceTarget = _cache.GetCache()[type];
                    JLog.Msg($"service instance with inject field = {serviceInstanceTarget.GetType()}");


                    JLog.Msg($"INJECTED?? {type} / {serviceInstanceTarget} / {instance}");

                    field.SetValue(serviceInstanceTarget, instance);
                    // await _container.BindAsync(type, serviceInstanceTarget);
                }
            }

            JLog.Msg($"( InjectDependenciesAsync STARTED...");

            // foreach (var type in assembly.GetTypes())
            // {
            //     var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //
            //     foreach (var field in fields)
            //     {
            //         if (!Attribute.IsDefined(field, typeof(JInject))) continue;
            //
            //
            //         // var value = Cache.GetCache()[field.FieldType];
            //         // Debug.LogWarning("value =  " + value);
            //         Debug.LogWarning("fi type =  " + field.FieldType);
            //
            //         // bool isTypeEqual = field.FieldType == value.GetType();
            //         // bool isImplementInterface = value.GetType().GetInterfaces().Contains(field.FieldType);
            //
            //         // if (!isTypeEqual && !isImplementInterface) continue;
            //
            //         // if (!isTypeEqual) continue;
            //
            //         Debug.LogWarning($"TYPE WITH INJECT = {type}");
            //
            //         // Debug.LogWarning($"Inject here: {type} -> {field.FieldType} -> {value.GetType()}");
            //
            //
            //         // field.SetValue(Activator.CreateInstance(field.FieldType), value);
            //         //
            //         // // for logging
            //         // var val = value.GetType().ToString().Split('.').Last();
            //         // var tar = value.ToString().Split('.').Last();
            //         // JLog.Msg($"Injected. {val} to {tar}");
            //     }
            // }

            JLog.Msg($"( InjectDependenciesAsync FINISHED...");

            await Task.CompletedTask;
            // object target;
            // foreach (var type in assembly.GetTypes())
            // {
            //     var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //
            //     foreach (var fieldInfo in fields)
            //     {
            //         if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;
            //
            //         JLog.Msg("INJECT HERE = " + type);
            //
            //
            //         var injectType = fieldInfo.FieldType;
            //         GetCache().TryGetValue(injectType, out var injectValue);
            //
            //         JLog.Builder().AddLine(injectType).AddLine(injectValue).Build();
            //
            //
            //         if (GetCache().TryGetValue(injectType, out var value))
            //         {
            //             Debug.Log("Return from cache");
            //             injectValue = value;
            //         }
            //
            //         target = GetCache()[type];
            //         Debug.Log("Not in cache");
            //
            //         Debug.LogWarning("======");
            //         Debug.Log(fieldInfo);
            //         // Debug.Log(target);
            //         Debug.LogWarning(injectType);
            //         Debug.LogWarning(injectValue);
            //         Debug.LogWarning("======");
            //         fieldInfo.SetValue(target, injectValue);
            //
            //         Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
            //     }
            // }
        }
    }
}
// private void Bind(Type type, object imp)
// {
//     if (_instancesCache.TryAdd(type, imp)) Debug.Log("Added to cache!");
// }

// public T InjectDependencies<T>() where T : class, new()
// {
//     Debug.Log("INJECT THIS = " + typeof(T));
//     var assembly = Assembly.GetAssembly(typeof(T));
//
//     Debug.LogWarning(assembly);
//
//     T target = new T();
//
//     foreach (var type in assembly.GetTypes())
//     {
//         var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
//
//         foreach (var fieldInfo in fields)
//         {
//             if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;
//
//             var injectType = fieldInfo.FieldType;
//             // var injectValue = _serviceFactory.GetService(injectType);
//
//             Debug.LogWarning(injectType);
//             // Debug.LogWarning(injectValue);
//
//
//             if (_servicesCache.TryGetValue(injectType, out var value))
//             {
//                 Debug.Log("Return from cache");
//                 // injectValue = value;
//             }
//
//             Debug.Log("Not in cache");
//
//             // Bind(injectType, injectValue);
//             Debug.LogWarning("======");
//             Debug.Log(fieldInfo);
//             Debug.Log(target);
//             Debug.LogWarning(injectType);
//             // Debug.LogWarning(injectValue);
//             Debug.LogWarning("======");
//             // fieldInfo.SetValue(target, injectValue);
//
//             Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
//         }
//     }
//
//     return target;
// }