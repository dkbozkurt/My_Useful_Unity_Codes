using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

// ReSharper disable UnusedMember.Local
// ReSharper disable MemberCanBePrivate.Global

namespace FlatKit {
#if FLAT_KIT_DEV
[CreateAssetMenu(fileName = "Readme", menuName = "FlatKit/Internal/Readme", order = 0)]
#endif // FLAT_KIT_DEV

[ExecuteAlways]
public class FlatKitReadme : ScriptableObject {
    [NonSerialized]
    public bool FlatKitInstalled;

    [NonSerialized]
    public readonly string FlatKitVersion = "3.7.0";

    [NonSerialized]
    public bool? UrpInstalled;

    [NonSerialized]
    [CanBeNull]
    public string PackageManagerError;

    [NonSerialized]
    public string UrpVersionInstalled = "N/A";

    [NonSerialized]
    public string UnityVersion = Application.unityVersion;

    private const string UrpPackageID = "com.unity.render-pipelines.universal";

    private static readonly GUID StylizedShaderGuid = new GUID("bee44b4a58655ee4cbff107302a3e131");

    public void Refresh() {
        UrpInstalled = false;
        FlatKitInstalled = false;
        PackageManagerError = null;

        PackageCollection packages = GetPackageList();
        foreach (PackageInfo p in packages) {
            if (p.name == UrpPackageID) {
                UrpInstalled = true;
                UrpVersionInstalled = p.version;
            }
        }

        string path = AssetDatabase.GUIDToAssetPath(StylizedShaderGuid.ToString());
        var flatKitSourceAsset = AssetDatabase.LoadAssetAtPath<Shader>(path);
        FlatKitInstalled = flatKitSourceAsset != null;

        UnityVersion = Application.unityVersion;
    }

    private PackageCollection GetPackageList() {
        var listRequest = Client.List(true);

        while (listRequest.Status == StatusCode.InProgress) { }

        if (listRequest.Status == StatusCode.Failure) {
            PackageManagerError = listRequest.Error.message;
            Debug.LogWarning("[Flat Kit] Failed to get packages from Package Manager.");
            return null;
        }

        return listRequest.Result;
    }
}
}