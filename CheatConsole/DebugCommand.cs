// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CheatConsole
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=VzOEM-4A2OM&ab_channel=GameDevGuide
    /// </summary>
    public class DebugCommandBase
    {
        private string _commandId;
        private string _commandDescription;
        private string _commandFormat;
        
        public string commandId { get { return _commandId; } }
        public string commandDescription { get { return _commandDescription; } }
        public string commandFormat { get { return _commandFormat; } }

        public DebugCommandBase(string id, string description, string format)
        {
            _commandId = id;
            _commandDescription = description;
            _commandFormat = format;
        }
    }
    
    public class DebugCommand : DebugCommandBase
    {
        private Action _command;
        
        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }

    public class DebugCommand<T> : DebugCommandBase
    {
        private Action<T> _command;
        
        public DebugCommand(string id, string description, string format,Action<T> command) : base(id, description, format)
        {
            _command = command;
        }

        public void Invoke(T value)
        {
            _command.Invoke(value);
        }
    }
}
