// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    /// COMMAND PATTERN
    /// </summary>
    
    // Concrete Command 
    public class ChangeColorCommand : ICommand
    {
        private Lightbulb _lightbulb;

        // stored previous color
        private Color _previousColor;
        
        public ChangeColorCommand(Lightbulb lightbulb)
        {
            _lightbulb = lightbulb;
            _previousColor = lightbulb.GetComponent<Renderer>().material.color;
        }

        public void Execute()
        {
            _lightbulb.SetRandomLightColor();
        }

        public void Undo()
        {
            _lightbulb.SetLightColor(_previousColor);
        }
    }
}