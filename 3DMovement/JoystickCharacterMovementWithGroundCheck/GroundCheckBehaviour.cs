using System;
using CpiTemplate.Game.Creative.Scripts.Controller;
using CpiTemplate.Game.Playable.Scripts.Controllers;
using UnityEngine;

namespace CpiTemplate.Game.Creative.Scripts.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class GroundCheckBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterMovementController _characterMovementController;
        
        private void OnTriggerEnter(Collider other)
        {
            _characterMovementController.SetGrounded(true);
        }

        private void OnTriggerExit(Collider other)
        {
            _characterMovementController.SetGrounded(false);
        }
    }
}
