using System;
using System.Collections;
using System.Collections.Generic;
using Game.Playable.Scripts.PreScripts;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public enum InteractionType
    {
        Default,
        OnFingerUp
    }

    [SerializeField] private EndCardController endCardController;

    [LunaPlaygroundField("Interaction Type", 0, "Interaction Settings")] [SerializeField]
    private InteractionType interactionType;

    private void Update()
    {
        if (interactionType == InteractionType.OnFingerUp)
        {
            if (Input.GetMouseButtonUp(0))
            {
                endCardController.OpenEndCard();
                DoAfterTime(1.5f);
            }
        }
    }

    private void DoAfterTime(float seconds)
    {
        StartCoroutine(Do());

        IEnumerator Do()
        {
            yield return new WaitForSeconds(seconds);
            endCardController.CloseEndCard();
        }
    }
}
