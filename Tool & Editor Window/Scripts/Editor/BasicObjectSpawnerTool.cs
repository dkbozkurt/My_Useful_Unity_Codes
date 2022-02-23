using System;
using UnityEngine;
using UnityEditor;
using Random = System.Random;

/// <summary>
/// This script must sit under "Editor" named folder. To not include into the game.
/// </summary>

public class BasicObjectSpawnerTool : EditorWindow
{
    #region Variables

    private string objectBaseName = "";
    private int objectID = 1;
    private GameObject objectToSpawn;
    private float objectScale;
    private float spawnRadius = 5f;

    #endregion

    [MenuItem("Tools/My Tools/Basic Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BasicObjectSpawnerTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Object",EditorStyles.boldLabel);

        objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn",objectToSpawn,typeof(GameObject),false) as GameObject;

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnObject();
        }
        
    }
    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Error: Please assign an object to be spawned");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Error: Please enter a base name for the object");
            return;
        }

        Vector2 spawnCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);


        GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }
}
