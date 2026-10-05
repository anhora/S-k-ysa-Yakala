using UnityEditor;

public static class ForceIOSBuildNumber
{
#if UNITY_CLOUD_BUILD
    public static void SetBuildNumber(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        PlayerSettings.iOS.buildNumber = manifest.GetValue<int>("buildNumber").ToString();
    }
#else
    public static void SetBuildNumber()
    {
        PlayerSettings.iOS.buildNumber = "1";
    }
#endif
}