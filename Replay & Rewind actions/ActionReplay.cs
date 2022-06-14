// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Add this script into prefab or object that would be effected by replay mode.
/// 
/// Ref : https://www.youtube.com/watch?v=R8RinJDzhf8&ab_channel=KetraGames
/// </summary>

public class ActionReplay : MonoBehaviour
{
    private bool _isInReplayMode;
    private float _currentReplayIndex;
    private float _indexChangeRate;
    
    private Rigidbody _rigidbody;
    private List<ActionReplayRecord> _actionReplayRecords = new List<ActionReplayRecord>();

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Enter and exit from replay mode.
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isInReplayMode = !_isInReplayMode;

            if (_isInReplayMode)
            {
                SetTransform(0);
                _rigidbody.isKinematic = true;
            }
            else
            {
                SetTransform(_actionReplayRecords.Count-1);
                _rigidbody.isKinematic = false;
            }
        }

        _indexChangeRate = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _indexChangeRate = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _indexChangeRate = -1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _indexChangeRate *= 0.5f;
        }
    }

    private void FixedUpdate()
    {
        if (!_isInReplayMode)
        {
            _actionReplayRecords.Add(new ActionReplayRecord {position = transform.position, rotation = transform.rotation});    
        }
        else
        {
            float nextIndex = _currentReplayIndex + _indexChangeRate;
            if (nextIndex < _actionReplayRecords.Count && nextIndex >= 0) SetTransform(nextIndex);
        }
        
    }

    private void SetTransform(float index)
    {
        _currentReplayIndex = index;
        ActionReplayRecord actionReplayRecord = _actionReplayRecords[(int)index];

        transform.position = actionReplayRecord.position;
        transform.rotation = actionReplayRecord.rotation;
    }
}