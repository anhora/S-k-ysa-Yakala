using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class ForceIOSBuildNumber : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.iOS)
        {
            PlayerSettings.iOS.buildNumber = "2";
            PlayerSettings.bundleVersion = "1.0";

            UnityEngine.Debug.Log(
                "FORCE IOS BUILD NUMBER: Version = 1.0, Build = 2"
            );
        }
    }
}