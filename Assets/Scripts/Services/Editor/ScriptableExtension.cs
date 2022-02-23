using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public static class ScriptableExtension
    {
        public static void Save(this ScriptableObject targetScriptable)
        {
            EditorUtility.SetDirty(targetScriptable);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SaveAsAtFolder(this ScriptableObject targetScriptable, string fileName, string folderPath)
        {
            AssetDatabase.CreateAsset(targetScriptable, folderPath + Path.DirectorySeparatorChar + fileName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SaveAsAtSamePathOf(this ScriptableObject targetScriptable, string fileName, string pathOfAssetAtSameFolder)
        {
            var folderPath = Path.GetDirectoryName(pathOfAssetAtSameFolder);;
            targetScriptable.SaveAsAtFolder(fileName, folderPath);
        }
    }

    public class ScriptableExt
    {
        public static IEnumerable<T> GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
            var returnList = new T[guids.Length];
            for(var i =0; i<guids.Length; i++) 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                returnList[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return returnList;
        }
    }
}
