// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lerping in Unity
/// 
/// By using .Lerp, we can change for example transform properties of an object, between current and target values.
///
/// Ref: https://www.youtube.com/watch?v=JS7cNHivmHw
/// </summary>

public class ObjectLerping : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private float speed = 0.5f;
    
    private float _current, _target;

    private void Update()
    {
        DoLerp();
    }

    private void DoLerp()
    {
        // To change Moving way
        if (Input.GetKeyDown(KeyCode.C)) _target = _target == 0 ? 1 : 0;
        
        _current = Mathf.MoveTowards(_current, _target, speed * Time.deltaTime);

        transform.position = Vector3.Lerp(Vector3.zero, targetPosition, _curve.Evaluate(_current));
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(targetRotation), _curve.Evaluate(_current));
        transform.localScale = Vector3.Lerp(Vector3.one, targetScale, _curve.Evaluate(_current));
        // Following line helps to catch targetScale at the middle of the path then turn back to starting scale value at the end of the path.
        //transform.localScale = Vector3.Lerp(Vector3.one, targetScale, _curve.Evaluate(Mathf.PingPong(_current,0.5f)*2));

    }
    
}