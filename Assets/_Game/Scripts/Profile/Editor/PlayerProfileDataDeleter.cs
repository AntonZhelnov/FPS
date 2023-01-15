#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Profile.Editor
{
    public static class PlayerProfileDataDeleter
    {
        [MenuItem("PlayerProfileData/Delete")]
        private static void Delete()
        {
            var filePath = $"{Application.persistentDataPath}/{typeof(PlayerProfileData).Name}.dat";
            File.Delete(filePath);
        }
    }
}
#endif