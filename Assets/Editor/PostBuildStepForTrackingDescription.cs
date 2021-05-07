using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class PostBuildStepForTrackingDescription
{
    const string k_TrackingDescription = "好みに合わせた広告を表示するために使用されます。";

    [PostProcessBuild(0)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            AddPListValues(pathToXcode);
        }
    }

    static void AddPListValues(string pathToXcode)
    {
        string plistPath = pathToXcode + "/Info.plist";

        PlistDocument plistObj = new PlistDocument();

        plistObj.ReadFromString(File.ReadAllText(plistPath));

        PlistElementDict plistRoot = plistObj.root;

        plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);

        File.WriteAllText(plistPath, plistObj.WriteToString());
    }
}
