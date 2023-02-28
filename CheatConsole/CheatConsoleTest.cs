// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using CpiTemplate.Game.Scripts;
using UnityEngine;

namespace CheatConsole
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=VzOEM-4A2OM&ab_channel=GameDevGuide
    /// </summary>
    public class CheatConsoleTest : SingletonBehaviour<CheatConsoleTest>
    {
        public string PlayerName
        {
            get => _playerName;
            
            set
            {
                _playerName = value;
                Debug.Log("Player name changed to : " + _playerName);
            }
        }

        private int _money=0;

        private string _playerName = "player";

        public void Test_KillAllMethod()
        {
            Debug.Log("KillAllMethod called !!!");
        }

        public void Test_Rosebud()
        {
            Debug.Log("Rosebud called !!!");
        }

        public void Test_Spawn()
        {
            Debug.Log("Spawn called !!!");
        }

        public void Test_MoneySet(int moneyToSet)
        {
            _money = moneyToSet;
            Debug.Log($"Money set called\n Money = {_money}");
        }
        

        protected override void OnAwake()
        { }
    }
}
