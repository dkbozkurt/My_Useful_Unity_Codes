// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    /// COMMAND PATTERN
    ///
    /// This class will store list of commands.
    ///
    /// Ref : https://www.youtube.com/watch?v=oLRINAn0cuw
    /// </summary>
    
    // Lists of commands for client
    public class LightApp
    {
        private Stack<ICommand> _commandList;

        public LightApp()
        {
            _commandList = new Stack<ICommand>();
        }

        public void AddCommand(ICommand newCommand)
        {
            newCommand.Execute();
            _commandList.Push(newCommand);
        }

        public void UndoCommand()
        {
            if(_commandList.Count <= 0) return;
            
            ICommand lastestCommand = _commandList.Pop();
            lastestCommand.Undo();
        }
    }
}