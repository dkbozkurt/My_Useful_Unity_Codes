using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.BasicsObjectSpawnTool.Scripts.Editor
{
    /// <summary>
    /// This script must sit under "Editor" named folder. To not include into the game.
    /// </summary>

    public class BasicObjectSpawnerTool : EditorWindow
    {
        #region Variables

        private string _objectBaseName = "";
        private int _objectID = 1;
        private GameObject _objectToSpawn;
        private float _objectScale;
        private float _spawnRadius = 5f;

        #endregion

        [MenuItem("Tools/My Tools/Basic Object Spawner")]
        public static void ShowWindow()
        {
            //GetWindow(typeof(BasicObjectSpawnerTool));
            GetWindow<BasicObjectSpawnerTool>("BasicsObjectSpawnerTool");
        }

        private void OnGUI()
        {
            GUILayout.Label("Spawn New Object",EditorStyles.boldLabel);

            _objectBaseName = EditorGUILayout.TextField("Base Name", _objectBaseName);
            _objectID = EditorGUILayout.IntField("Object ID", _objectID);
            _objectScale = EditorGUILayout.Slider("Object Scale", _objectScale, 0.5f, 3f);
            _spawnRadius = EditorGUILayout.FloatField("Spawn Radius", _spawnRadius);
            _objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn",_objectToSpawn,typeof(GameObject),false) as GameObject;

            if (GUILayout.Button("Spawn Object"))
            {
                SpawnObject();
            }
        
        }
        private void SpawnObject()
        {
            if (_objectToSpawn == null)
            {
                Debug.LogError("Error: Please assign an object to be spawned");
                return;
            }
            if (_objectBaseName == string.Empty)
            {
                Debug.LogError("Error: Please enter a base name for the object");
                return;
            }

            Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);


            GameObject newObject = Instantiate(_objectToSpawn, spawnPos, Quaternion.identity);
            newObject.name = _objectBaseName + _objectID;
            newObject.transform.localScale = Vector3.one * _objectScale;

            _objectID++;
        }
    }
}
