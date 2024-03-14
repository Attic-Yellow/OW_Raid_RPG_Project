using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    static void AssetBundleBuild()
    {
        var path = Application.streamingAssetsPath;

        if (Directory.Exists(path))
        {
            BuildPipeline.BuildAssetBundles(
                path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
    }
}
