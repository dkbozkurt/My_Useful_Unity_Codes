// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=2LA3BLqOw9g
/// </summary>

public class CompositeInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> interactableGameObjects = new List<GameObject>();

    public void Interact()
    {

        foreach (var interactableGameObject in interactableGameObjects)
        {
            var interactable = interactableGameObject.GetComponent<IInteractable>();
            if(interactable == null) continue;
            interactable.Interact();
        }
    }

}
