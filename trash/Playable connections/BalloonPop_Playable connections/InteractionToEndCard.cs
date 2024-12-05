using System;
using System.Collections;
using System.Collections.Generic;
using Game.Playable.Scripts.PreScripts;
using Game.Scripts.Controllers;
using UnityEngine;

public class InteractionToEndCard : MonoBehaviour
{
    public enum InteractionType
    {
        FirstTouch,
        OnRelease
    }

    [SerializeField] private EndCardController endCardController;
    [SerializeField] private BallController ballController;

    [LunaPlaygroundField("Check Interaction On/Off", 0, "Interaction Settings")] [SerializeField]
    private bool checkInteraction;
    
    [LunaPlaygroundField("Interaction Type", 1, "Interaction Settings")] [SerializeField]
    private InteractionType interactionType;

    [LunaPlaygroundFieldStep(0.25f)]
    [LunaPlaygroundField("Open end card after interaction type", 2, "Interaction Settings")]
    [Range(0, 7)]
    [SerializeField]
    private float timeToWait = 7;

    private bool isTouched = false;

    void Update()
    {
        if (checkInteraction && !isTouched)
            DecideInteraction();
        
    }

    private void DecideInteraction()
    {
        if (interactionType == InteractionType.FirstTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("FirstTouch");
                isTouched = true;
                DoAfterTime(timeToWait);    
            }
        }
        else if (interactionType == InteractionType.OnRelease)
        {
            if (ballController._isReleased)
            {
                Debug.Log("is Released");
                isTouched = true;
                DoAfterTime(timeToWait);    
            }
        }

    }

    private void DoAfterTime(float seconds)
    {
        StartCoroutine(Do());

        IEnumerator Do()
        {
            yield return new WaitForSeconds(seconds);
            endCardController.Enable();
        }
    }
}