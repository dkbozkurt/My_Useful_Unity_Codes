// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

namespace CheatConsole
{
    /// <summary>
    /// Cheat console works with old input system.
    /// 
    /// Ref : https://www.youtube.com/watch?v=VzOEM-4A2OM&ab_channel=GameDevGuide
    /// </summary>
    public class DebugController : MonoBehaviour
    {
        public static DebugCommand HELP;
        public static DebugCommand KILL_ALL;
        public static DebugCommand ROSEBUD;
        public static DebugCommand SPAWN;
        public static DebugCommand<int> SET_GOLD;
        public static DebugCommand<string> SET_NAME;

        private List<object> _commandList;

        #region Editor

        private bool _showConsole = false;
        private string _inputString;
        
        private bool _showHelp = false;
        private Vector2 _scroll;

        #endregion

        private void Awake()
        {
            AssignCommands();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnToggleDebug();
            }

            if (_showConsole && Input.GetKeyDown(KeyCode.Return))
            {
                OnReturn();
            }
        }
        
        private void AssignCommands()
        {
            HELP = new DebugCommand("help", "Shows a list of commands", "help", 
                () => { _showHelp = true; });
            
            KILL_ALL = new DebugCommand("kill_all", "Removes all enemies from the scene.", "kill_all",
                () => { CheatConsoleTest.Instance.Test_KillAllMethod(); });

            ROSEBUD = new DebugCommand("rosebud", "Adds 1000 coins.", "rosebud",
                () => { CheatConsoleTest.Instance.Test_Rosebud(); });
            
            SPAWN = new DebugCommand("spawn", "Spawn hero.", "spawn",
                () => { CheatConsoleTest.Instance.Test_Spawn(); });

            SET_GOLD = new DebugCommand<int>("set_gold", "Sets the amount of gold.", "set_gold <gold_amount>",
                (x) => { CheatConsoleTest.Instance.Test_MoneySet(x); });
            
            SET_NAME = new DebugCommand<string>("set_playerName", "ReSets the name of player.", "set_playerName <new_player_name>",
                (x) => { CheatConsoleTest.Instance.PlayerName = x; });


            _commandList = new List<object>
            {
                HELP,
                KILL_ALL,
                ROSEBUD,
                SPAWN,
                SET_GOLD,
                SET_NAME
            };
        }


        public void OnToggleDebug()
        {
            _showConsole = !_showConsole;
            _showHelp = false;
        }

        public void OnReturn()
        {
            if (_showConsole)
            {
                HandleInput();
                _inputString = "";
            }
        }

        private void OnGUI()
        {
            if (!_showConsole) return;

            float y = 0f;
            
            if (_showHelp)
            {
                GUI.Box(new Rect(0,y,Screen.width,100),"");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * _commandList.Count);

                _scroll = GUI.BeginScrollView(new Rect(0, y+ 5f, Screen.width, 90), _scroll, viewport);

                for (int i = 0; i < _commandList.Count; i++)
                {
                    DebugCommandBase command = _commandList[i] as DebugCommandBase;

                    string label = $"{command.commandFormat} - {command.commandDescription}";

                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                
                    GUI.Label(labelRect,label);
                }
            
                GUI.EndScrollView();

                y += 100;
            }
            
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            _inputString = GUI.TextField(new Rect(10, y+5f, Screen.width - 20f, 20), _inputString);
        }
        
        private void HandleInput()
        {
            string[] properties = _inputString.Split(' ');
            
            for (int i = 0; i < _commandList.Count; i++)
            {
                DebugCommandBase commandBase = _commandList[i] as DebugCommandBase;

                if (!_inputString.Contains(commandBase.commandId)) continue;
                
                if (_commandList[i] as DebugCommand != null)
                {
                    (_commandList[i] as DebugCommand).Invoke();
                }
                else if(_commandList[i] as DebugCommand<int> != null)
                {
                    (_commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (_commandList[i] as DebugCommand<string> != null)
                {
                    (_commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
            }
        }
    }
}