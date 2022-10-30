// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using UnityEditor; // Do not forget to import it.

/// <summary>
/// IMPORTANT: This script must sit under "Editor" named folder. To not include into the game.
///
/// Ref: https://www.youtube.com/watch?v=RInUu1_8aGw&t=242s&ab_channel=Brackeys
/// </summary>

// We are telling what script that we want this to be an editor for by using the following line.
// Here we are editing Script name called "Cube"
[CustomEditor(typeof(Cube))]
public class CubeEditor : Editor // Change MonoBehaviour to Editor.
{
    public override void OnInspectorGUI()
    {
        // Says do all of the original GUI code and then allow us to add stuff before or after that.
        base.OnInspectorGUI();

        // target default ref to gameObject, here target refers to the Cube
        Cube cube = (Cube) target;

        #region Size related

        GUILayout.Label("Oscillates around a base size.");
        
        // Slider added to change multiplier of the cube animation.
        cube.baseSize = EditorGUILayout.Slider("Size Multiplier",cube.baseSize, .1f, 2f);
        cube.transform.localScale = Vector3.one * cube.baseSize;

        #endregion
        
        // Between BeginHorizontal() and EndHorizontal(), components will layout horizontally instead of vertically.
        GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Generate Color"))
            {
                cube.GenerateColor();
            }
                    
            if (GUILayout.Button("Reset Color"))
            {
                cube.Reset();
            }

        GUILayout.EndHorizontal();
        
    }
    
}