#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Common.Utilities.Editor
{
    public class Dirtyneer : MonoBehaviour
    {
        [MenuItem("Assets/Tools/Set Dirty")]
        private static void SetDirty()
        {
            foreach (var selectedObject in Selection.objects)
                EditorUtility.SetDirty(selectedObject);
        }
    }
}
#endif