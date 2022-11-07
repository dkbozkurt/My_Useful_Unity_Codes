// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  
/// Attach this script on to the surface to be painted.
///
/// NOTE : SUITABLE FOR PLAYABLE BUILDS HOWEVER LOW PERFORMANCE
/// </summary>

[RequireComponent(typeof(Collider))]
public class Paintable : MonoBehaviour
{
    [SerializeField] private GameObject brush;
    [SerializeField] private float brushSize=0.1f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartPaint();
        }
    }

    private void StartPaint()
    {
        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
            var go = Instantiate(brush, hit.point + Vector3.up * 0.1f, Quaternion.identity,transform);
            go.transform.localScale = Vector3.one * brushSize;
        }
    }
}
