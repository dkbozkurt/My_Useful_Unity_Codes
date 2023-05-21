// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using CpiTemplate.Game.Scripts;
using UnityEngine;

namespace Game_Mechanics.BasicCarSimulator.Scripts.Controllers
{
    /// <summary>
    /// Ref : 
    /// </summary>
    public class InputController : SingletonBehaviour<InputController>
    {

        public float GetVerticalInput()
        {
            return Input.GetAxis("Vertical");
        }

        public float GetHorizontalInput()
        {
            return Input.GetAxis("Horizontal");
        }
        
        protected override void OnAwake()
        { }
    }
}
