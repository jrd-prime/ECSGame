using UnityEngine;

namespace Sources.Scripts.TestViews
{
    public class ModernView : IViews
    {
        public void ShowInfo()
        {
            Debug.LogWarning("Its modern view!");
        }
    }
}