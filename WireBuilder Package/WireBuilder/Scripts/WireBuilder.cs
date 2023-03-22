#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(WireController))]
[ExecuteInEditMode]
public class WireBuilder : Editor
{
    Vector3 position;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WireController wireController = (WireController)target;
        if (GUILayout.Button("Set Start"))
        {
            wireController.AddStar();
        }
        if (GUILayout.Button("Add Segment"))
        {
            wireController.AddSegment();
        }
        if (GUILayout.Button("Set End"))
        {
            wireController.AddEnd();
        }
        if (GUILayout.Button("Set Plug"))
        {
            wireController.AddPlug();
        }
        if (GUILayout.Button("Clear"))
        {
            wireController.Clear();
        }
        if (GUILayout.Button("Undo"))
        {
            wireController.Undo();
        }
        if (GUILayout.Button("Render Wire"))
        {
            wireController.RenderWireMesh();
        }
        if (GUILayout.Button("Finish no physics wire"))
        {
            wireController.FinishNoPhysicsWire();
        }
    }
    public void Update()
    {
        OnSceneGUI();
    }

    void OnSceneGUI()
    {
        Event e = Event.current;

        // check mouse down event
        if (e.type != EventType.MouseDown)
        {
            return;
        }

        // check right mouse button
        if (e.button != 1)
        {
            return;
        }

        // create OnSceneGUI ray
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        // check hit
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        // Use ray cast hit here
        //Debug.Log("MousePos: " + hit.point);

        position = hit.point;
        WireController wireController = (WireController)target;
        wireController.SetPosition(position);

        // tell event to no longer propergate
        e.Use();
    }
}
#endif