// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    /// COMMAND PATTERN
    ///
    /// Ref : https://www.youtube.com/watch?v=oLRINAn0cuw
    /// </summary>
    
    // Concrete Command 
    public class TogglePowerCommand : ICommand
    {
        private Lightbulb _lightbulb;
        // Constructor
        public TogglePowerCommand(Lightbulb lightbulb)
        {
            _lightbulb = lightbulb;
        }
        
        public void Execute()
        {
            _lightbulb.TogglePower();
        }
        
        public void Undo()
        {
            _lightbulb.TogglePower();   
        }
    }
}