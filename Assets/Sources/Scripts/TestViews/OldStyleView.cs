using UnityEngine;

namespace Sources.Scripts.TestViews
{
    public class OldStyleView : IViews
    {
        public void ShowInfo()
        {
            Debug.LogWarning("Its oldstyle view!");
        }
    }
}