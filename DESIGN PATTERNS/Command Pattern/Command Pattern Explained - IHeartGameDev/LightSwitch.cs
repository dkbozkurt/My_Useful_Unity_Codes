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
    
    // Invoker
    public class LightSwitch
    {
        private ICommand _onCommand;

        public LightSwitch(ICommand onCommand)
        {
            _onCommand = onCommand;
        }

        public void TogglePower()
        {
            _onCommand.Execute();
        }
    }
}