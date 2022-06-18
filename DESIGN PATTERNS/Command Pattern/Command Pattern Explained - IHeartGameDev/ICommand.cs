// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    /// COMMAND PATTERN
    /// 
    /// Ref : https://www.youtube.com/watch?v=oLRINAn0cuw
    /// </summary>
    
    // Abstract command interface
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}