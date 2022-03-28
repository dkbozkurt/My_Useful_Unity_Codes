// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// This script need to attach onto the object that will be following the path points.
///
/// Use this script with "WaypointMoveController.cs"
/// Ref: https://www.youtube.com/watch?v=PLFQp0TvsK0
/// </summary>

public class Movable : MonoBehaviour
{
    [SerializeField] private float _speedMetersPerSecond = 25f;

    private Vector3? _destination;
    private Vector3 _startPosition;
    private float _totalLerpDuration;
    private float _elapsedLerpDuration;
    private Action _onCompleteCallback;

    private void Update()
    {
        if (_destination.HasValue == false) return;
        
        if(_elapsedLerpDuration >= _totalLerpDuration && _totalLerpDuration >0) return;

        _elapsedLerpDuration += Time.deltaTime;
        float percent = (_elapsedLerpDuration / _totalLerpDuration);
        Debug.Log($"{percent} = {_elapsedLerpDuration}/{_totalLerpDuration}");

        transform.position = Vector3.Lerp(_startPosition, _destination.Value, percent);
        
        if(_elapsedLerpDuration >= _totalLerpDuration) // Reached Position
            _onCompleteCallback?.Invoke();
    }

    public void MoveTo(Vector3 destination, Action onComplete = null)
    {
        var distanceToNextWaypoint = Vector3.Distance(transform.position, destination);
        _totalLerpDuration = distanceToNextWaypoint / _speedMetersPerSecond;

        _startPosition = transform.position;
        _destination = destination;
        _elapsedLerpDuration = 0f;
        _onCompleteCallback = onComplete;

    }



}
