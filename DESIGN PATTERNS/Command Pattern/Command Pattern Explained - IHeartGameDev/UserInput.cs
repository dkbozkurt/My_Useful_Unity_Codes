// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    /// COMMAND PATTERN
    ///
    /// Attach this script on to the input controller object that gonna control the process.
    ///
    /// Ref : https://www.youtube.com/watch?v=oLRINAn0cuw
    /// </summary>
    
    // CLIENT
    public class UserInput : MonoBehaviour
    {
        public Lightbulb _Lightbulb;
        private LightApp _lightApp;

        private void Start()
        {
            _lightApp = new LightApp();

        }

        private void Update()
        {
            // Add/Execute to invoker list and execute the command or undo command
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ICommand togglePowerCommand = new TogglePowerCommand(_Lightbulb);
                _lightApp.AddCommand(togglePowerCommand);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                ICommand changeColorCommand = new ChangeColorCommand(_Lightbulb);
                _lightApp.AddCommand(changeColorCommand);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                _lightApp.UndoCommand();
            }
        }
    }
}