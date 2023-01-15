using System;
using UnityEngine;

namespace Input
{
    public class ApplicationFocusReactor : MonoBehaviour
    {
#pragma warning disable CS0067
        public event Action<bool> ApplicationFocused;
#pragma warning restore CS0067


#if !UNITY_EDITOR
        private void OnApplicationFocus(bool isFocused)
        {
            ApplicationFocused?.Invoke(isFocused);
        }
#endif
    }
}