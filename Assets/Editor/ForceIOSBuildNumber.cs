using UnityEditor;

public static class ForceIOSBuildNumber
{
    [InitializeOnLoadMethod]
    private static void SetBuildNumber()
    {
        PlayerSettings.iOS.buildNumber = "1";
    }
}