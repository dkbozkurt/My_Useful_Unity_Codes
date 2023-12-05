using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class UnityPackages : KitButtonBase
    {
        private readonly string _animationBakerPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/AnimationBaker.unitypackage";
        
        private readonly string _doTweenPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/DoTween.unitypackage";
        
        private readonly string _enhancedHierarchyPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/EnhancedHierarchy.unitypackage";
        
        private readonly string _epicToonFXPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/EpicToonFX.unitypackage";
        
        private readonly string _meshBakerFXPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/MeshBaker.unitypackage";
        
        private readonly string _toonyColorsFreePackagePath =
            "Assets/PlayableAdsKit/UnityPackages/ToonyColorsFree.unitypackage";
        
        private readonly string _urp_simpleToonShaderPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/URP_SimpleToonShader.unitypackage";
        
        private readonly string _justdiceGEOPackagePath =
            "Assets/PlayableAdsKit/UnityPackages/UnityTranslationTool_v3_0.unitypackage";

        UnityPackages()
        {
            Name= "Unity Packages";
            DescriptionText = "Imports useful unity packages.";
        }

        public override void DrawImportButton() { }

        protected override void DrawDescriptionBody()
        {
            PackageGroup("Enhanced Hierarchy",_enhancedHierarchyPackagePath);
            PackageGroup("Do TWEEN",_doTweenPackagePath);
            PackageGroup("JustDice GEO",_justdiceGEOPackagePath);
            PackageGroup("Animation Baker",_animationBakerPackagePath);
            PackageGroup("Epic Toon FX",_epicToonFXPackagePath);
            PackageGroup("Mesh Baker",_meshBakerFXPackagePath);
            PackageGroup("Toony Colors Free",_toonyColorsFreePackagePath);
            PackageGroup("URP Simple Toon Shader",_urp_simpleToonShaderPackagePath);
        }

        private void PackageGroup(string labelName, string packagePath, string buttonName = "Import")
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label(labelName,GetDefaultLabelTextStyle());
            if (GUILayout.Button(buttonName,GUILayout.MaxWidth(120)))
            {
                ImportUnityPackage(packagePath);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void ImportUnityPackage(string packagePath)
        {
            if (!string.IsNullOrEmpty(packagePath) && System.IO.File.Exists(packagePath))
            {
                AssetDatabase.ImportPackage(packagePath, true);
            }
            else
            {
                Debug.LogError("Invalid file path or file does not exist: " + packagePath);
            }
        }
        
        private GUIStyle GetDefaultLabelTextStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 14,
                normal = { textColor = Color.white }
            };
        }
    }
}
