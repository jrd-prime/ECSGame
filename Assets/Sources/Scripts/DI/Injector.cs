using System;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.DI.Interface;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class Injector : IInjector, IFieldsInjectable
    {
        [JManualInject] private IMyContainer _container;
        [JManualInject] private ICache _cache;

        public bool IsFieldsInjected() => _container != null && _cache != null;

        public async Task InjectDependenciesAsync(Assembly assembly)
        {
            JLog.Msg($"( InjectDependenciesAsync STARTED...");
            
            foreach (var type in assembly.GetTypes())                        
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    JLog.Msg($"= = = = = =");
                    if (!Attribute.IsDefined(field, typeof(JInject))) continue;

                    JLog.Msg($"type = {type}");
                    JLog.Msg($"field = {field}");
                    JLog.Msg($"field has attr for inject");

                    var target = field.FieldType;
                    JLog.Msg($"target (field type) = {target}");

                    Debug.LogWarning("count cache = " + _cache.GetCache().Count);

                    var instance = _cache.GetCache()[target];

                    JLog.Msg($"instance from cache = {instance}");

                    var serviceInstanceTarget = _cache.GetCache()[type];
                    JLog.Msg($"service instance with inject field = {serviceInstanceTarget.GetType()}");


                    JLog.Msg($"INJECTED?? {type} / {serviceInstanceTarget} / {instance}");

                    field.SetValue(serviceInstanceTarget, instance);
                }
            }
            
            JLog.Msg($"( InjectDependenciesAsync FINISHED...");

            await Task.CompletedTask;
        }
    }
}