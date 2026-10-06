using UnityEditor;
using UnityEngine;

public static class ForceIOSBuildNumber
{
    public static void SetBuildNumberBeforeExport()
    {
        PlayerSettings.iOS.buildNumber = "1";
        PlayerSettings.bundleVersion = "1.0";
        Debug.Log("iOS build number export öncesi zorlandı: 1");
    }
}