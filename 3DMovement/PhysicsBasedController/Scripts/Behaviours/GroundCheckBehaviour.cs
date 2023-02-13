// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using _3DMovement.PhysicsBasedController.Scripts.Controllers;
using UnityEngine;

namespace _3DMovement.PhysicsBasedController.Scripts.Behaviours
{
    /// <summary>
    /// Instead of checking if colliding with player, we can assign layers and from the physics, we can disable detection
    /// between player and ground check.
    ///  
    ///  Ref : https://www.youtube.com/watch?v=1LtePgzeqjQ&ab_channel=PotatoCode
    /// </summary>
    public class GroundCheckBehaviour : MonoBehaviour
    {
        [SerializeField]private PhysicsBasedCharacterController _physicsBasedCharacterController;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == _physicsBasedCharacterController.gameObject) return;

            _physicsBasedCharacterController.SetGrounded(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject == _physicsBasedCharacterController.gameObject) return;

            _physicsBasedCharacterController.SetGrounded(false);
        }
    }
}
