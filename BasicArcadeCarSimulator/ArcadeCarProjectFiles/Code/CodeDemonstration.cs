using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDemonstration : MonoBehaviour {

    float Value;

    void Update() {
        Vector3 a = Vector3.up;
        Vector3 b = Vector3.right;

        // change value between 0 and 1 over time
        Value = Mathf.Sin(Time.time * 6.28f * 0.2f) * 0.5f + 0.5f;
        Vector3 c = Vector3.Lerp(a, b, Value);

        // make the length always one
        c.Normalize();

        // draw vectors
        Debug.DrawRay(Vector3.zero, a, Color.black);
        Debug.DrawRay(Vector3.zero, b, Color.white);
        Debug.DrawRay(Vector3.zero, c, Color.green);
    }

    private void OnGUI() {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 100), Value.ToString("F1"));
    }
}
