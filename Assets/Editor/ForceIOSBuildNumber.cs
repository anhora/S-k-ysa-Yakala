using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class ForceIOSBuildNumber
{
    [InitializeOnLoadMethod]
    private static void ForceBuildNumber()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            PlayerSettings.iOS.buildNumber = "1";
            PlayerSettings.bundleVersion = "1.0";
            Debug.Log("iOS build number zorlandı: 1");
        }
    }
}