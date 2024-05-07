using System;
using UnityEngine;

namespace ECSGame.Scripts.Core.Annotation
{
    [AttributeUsage(AttributeTargets.Field)]
    public class JInject : Attribute
    {
        public JInject()
        {
            Debug.LogWarning("Inject???");
        }
    }
}