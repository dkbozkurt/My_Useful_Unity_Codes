// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///
/// This script need to attach onto the parent object of the way(path) points. 
///
/// Use this script with "Movable.cs"
/// Ref: https://www.youtube.com/watch?v=PLFQp0TvsK0
/// </summary>

public class WaypointMoveController : MonoBehaviour
{
    [SerializeField] private Movable target;

    private List<Transform> _wayPoints;

    private int _nextWaypointIndex;

    private void OnEnable()
    {
        _wayPoints = GetComponentsInChildren<Transform>().ToList();
        _wayPoints.RemoveAt(0); // remove self
        MoveToNextWayPoint();
    }

    private void MoveToNextWayPoint()
    {
        var targetWaypointTransform = _wayPoints[_nextWaypointIndex];
        target.MoveTo(targetWaypointTransform.position, MoveToNextWayPoint);
        
        target.transform.LookAt(targetWaypointTransform.position);
        
        _nextWaypointIndex++;

        if (_nextWaypointIndex >= _wayPoints.Count)
            _nextWaypointIndex = 0;
    }
}
