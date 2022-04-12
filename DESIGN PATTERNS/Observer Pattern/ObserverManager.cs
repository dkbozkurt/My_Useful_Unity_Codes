// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Observer Manager
///
/// Add this script onto empty gameObject in the scene.
///
/// NOTE:
/// SUBJECTS MUST SIGN IN BEFORE THAN THE OBSERVERS, SO WE NEED TO ADJUST QUEUE OF IT
/// EDIT > PROJECT SETTINGS > SCRIPT EXECUTION ORDER AND ADD FIRSTLY (FOR THIS CASE) MOVEMENTPANEL THEN MOVEMENTSYSTEM.
/// 
///
/// Ref: https://www.youtube.com/watch?v=hnxzYdnjH1U
/// </summary>

public enum NotificationType
{
    ForwardButton,
    BackButton,
    LeftButton,
    RightButton
}

public enum SubjectType
{
    MovementPanel,
}

public class ObserverManager : MonoBehaviour
{
    #region Basic Singleton

    private static ObserverManager _instance = null;
    public static ObserverManager Instance => _instance;

    #endregion

    private List<Subject> _subjects = null;

    private void Awake()
    {
        _instance = this;
    }
    
    public void RegisterSubject(Subject subject)
    {
        if (_subjects == null) _subjects = new List<Subject>();
        
        _subjects.Add(subject);
    }

    public void RegisterObserver(Observer observer, SubjectType subjectType)
    {
        foreach (var subject in _subjects)
        {
            if (subject.SubjectType == subjectType)
            {
                subject.RegisterObserver(observer);
            }
        }
    }
    
}


