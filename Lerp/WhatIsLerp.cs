// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Int Lerp
/// 
/// Returns the value corresponding to the lerp value between the start and end values.
///
/// Helps to give point information between A and B points.
/// 
/// Ref: https://www.youtube.com/watch?v=BAahPJS-G4c
/// </summary>

// Helps to work without Play mode.
[ExecuteInEditMode]
public class WhatIsLerp : MonoBehaviour
{
    #region IntLerp
    
    [SerializeField] private int startValue = 0;
    [SerializeField] private int endValue = 100;
    [SerializeField] private int currentValue = 0;
    
    [SerializeField] private bool isUnclamped = false;
    #endregion
    
    [SerializeField] private Transform startTransform = null,endTransform =null;

    [SerializeField] [Range(0f, 1f)] private float lerpValue = 0;

    #region ColorLerp

    [SerializeField] private Color startColor = Color.white, endColor = Color.black;
    private MeshRenderer _meshRenderer;

    #endregion

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // IntLerp();
        //
        // PositionLerp();
        //
        // RotationLerp();
        //
        // ScaleLerp();
        //
        // ColorLerp();

    }

    private void IntLerp()
    {
        if (isUnclamped)
        {
            // LerpUnclamped: Works with negative and higher than 1 values relative to lerp logic.
            currentValue = (int)Mathf.LerpUnclamped(startValue, endValue, lerpValue);
        }
        else
        {
            // It will only know the values between [0,1]
            currentValue = (int)Mathf.Lerp(startValue, endValue, lerpValue);
        }
    }
    
    private void PositionLerp()
    {
        transform.position = Vector3.Lerp(startTransform.position, endTransform.position, lerpValue);
    }
    
    private void RotationLerp()
    {
        transform.rotation = Quaternion.Lerp(startTransform.rotation,endTransform.rotation,lerpValue);
    }

    private void ScaleLerp()
    {
        transform.localScale = Vector3.Lerp(startTransform.localPosition, endTransform.localPosition, lerpValue);
    }

    private void ColorLerp()
    {
        _meshRenderer.material.color = Color.Lerp(startColor, endColor, lerpValue);
    }
    
}
