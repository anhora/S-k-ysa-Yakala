using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public static class ForceIOSBuildNumber
{
    public static void SetBuildNumberAfterExport(string exportPath)
    {
        string plistPath = Path.Combine(exportPath, "Info.plist");

        if (!File.Exists(plistPath))
        {
            UnityEngine.Debug.LogError("Info.plist bulunamadı: " + plistPath);
            return;
        }

        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.root.SetString("CFBundleVersion", "1");

        plist.WriteToFile(plistPath);

        UnityEngine.Debug.Log("iOS CFBundleVersion zorlandı: 1");
    }
}