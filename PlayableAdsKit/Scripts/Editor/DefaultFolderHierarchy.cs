using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayableAdsKit.Scripts.Editor
{
    public class DefaultFolderHierarchy : KitButtonBase
    {
        private readonly string _mainFolderPath = "Game";
        
        private readonly string _animationsFolderPath = "Animations";
        private readonly string _audiosFolderPath = "Audios";
        private readonly string _fontsFolderPath = "Fonts";
        private readonly string _materialsFolderPath = "Materials";
        private readonly string _modelsFolderPath = "Models";
        private readonly string _particlesFolderPath = "Particles";
        private readonly string _prefabsFolderPath = "Prefabs";
        private readonly string _scenesFolderPath = "Scenes";
        private readonly string _scriptsFolderPath = "Scripts";
        private readonly string _texturesFolderPath = "Textures";
        
        private readonly string _prefabsUIFolderPath = "UI";
        private readonly string _scriptsBehavioursFolderPath = "Behaviours";
        private readonly string _scriptsControllersFolderPath = "Controllers";
        private readonly string _scriptsHelpersFolderPath = "Helpers";
        private readonly string _scriptsManagersFolderPath = "Managers";

        private string[] _folderPaths;
        private string[] _subFolderPaths;

        DefaultFolderHierarchy()
        {
            Name = "Default Folder Hierarchy";
            DescriptionText = "Setups default folder hierarchy for unity.";

            _folderPaths = new string[10]
            {
                _animationsFolderPath,
                _audiosFolderPath,
                _fontsFolderPath,
                _materialsFolderPath,
                _modelsFolderPath,
                _particlesFolderPath,
                _prefabsFolderPath,
                _scenesFolderPath,
                _scriptsFolderPath,
                _texturesFolderPath,
            };
            
        }

        protected override void RunAction()
        {
            if (CheckIfFolderExists(_mainFolderPath))
            {
                Debug.LogError($"{_mainFolderPath} already exists! Folder creation process stopped.");
                return;
            }

            CreateSubFolder("Assets", _mainFolderPath);
                
            for (int i = 0; i < _folderPaths.Length; i++)
            {
                CreateSubFolder("Assets/Game", _folderPaths[i]);
            }
            
            CreateSubFolder("Assets/Game/Prefabs",_prefabsUIFolderPath);
            
            CreateSubFolder("Assets/Game/Scripts",_scriptsBehavioursFolderPath);
            CreateSubFolder("Assets/Game/Scripts",_scriptsControllersFolderPath);
            CreateSubFolder("Assets/Game/Scripts",_scriptsManagersFolderPath);
            CreateSubFolder("Assets/Game/Scripts",_scriptsHelpersFolderPath);

            CreateEmptyScene("Assets/Game/Scenes/","Main");
        }
        
        private bool CheckIfFolderExists(string folderPath)
        {
            return AssetDatabase.IsValidFolder(folderPath);
        }
        
        public static void CreateSubFolder(string parentFolderPath, string subFolderName)
        {
            if (!AssetDatabase.IsValidFolder(parentFolderPath))
            {
                Debug.LogError("Parent folder does not exist: " + parentFolderPath);
                return;
            }
            
            string newFolderPath = System.IO.Path.Combine(parentFolderPath, subFolderName);

            newFolderPath = newFolderPath.Replace("\\", "/");

            if (!AssetDatabase.IsValidFolder(newFolderPath))
            {
                AssetDatabase.CreateFolder(parentFolderPath, subFolderName);
            }
            else
            {
                Debug.LogWarning("Subfolder already exists: " + newFolderPath);
            }

            AssetDatabase.Refresh();
        }
        
        private static void CreateEmptyScene(string folderPath, string sceneName)
        {
            if (!folderPath.EndsWith("/"))
                folderPath += "/";

            string scenePath = folderPath + sceneName + ".unity";

            if (System.IO.File.Exists(scenePath))
            {
                Debug.LogError("A scene with this name already exists: " + sceneName);
                return;
            }

            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            EditorSceneManager.SaveScene(newScene, scenePath);
        }
    }
}
