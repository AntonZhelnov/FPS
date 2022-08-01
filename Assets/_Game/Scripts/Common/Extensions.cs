using UnityEngine;

namespace Common
{
    public static class Extensions
    {
        public static void SetLayer(
            this Transform transform,
            int layer)
        {
            transform.gameObject.layer = layer;
            foreach (Transform child in transform)
                child.SetLayer(layer);
        }
    }
}