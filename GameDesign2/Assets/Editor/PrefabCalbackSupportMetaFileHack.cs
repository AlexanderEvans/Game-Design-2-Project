using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PrefabCalbackSupportMetaFileHack : AssetModificationProcessor
{
    public static List<string> newAssets = new List<string>();
    static void OnWillCreateAsset(string aMetaAssetPath)
    {
        string assetPath = aMetaAssetPath.Substring(0, aMetaAssetPath.Length - 5);
        newAssets.Add(assetPath);
    }
}

class PrefabCalbackSupportMetaFileHackPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (PrefabCalbackSupportMetaFileHack.newAssets.Count == 0)
            return;
        foreach (var str in importedAssets)
        {
            if (PrefabCalbackSupportMetaFileHack.newAssets.Contains(str))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath(str, typeof(GameObject)) as GameObject;
                if (obj == null)
                    continue;
                IOnPrefabCreated comp = obj.GetComponent<IOnPrefabCreated>();
                if (comp == null)
                    continue;
                comp.OnPrefabCreated();
            }
        }
        PrefabCalbackSupportMetaFileHack.newAssets.Clear();
    }
}