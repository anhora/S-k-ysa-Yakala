using UnityEditor;

public static class ForceIOSBuildNumber
{
    public static void SetBuildNumber()
    {
        PlayerSettings.iOS.buildNumber = "1";
    }
}