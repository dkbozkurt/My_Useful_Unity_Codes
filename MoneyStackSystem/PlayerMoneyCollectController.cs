using CpiTemplate.Game.Scripts;
using UnityEngine;

namespace MoneyStackSystem
{
    
    public class PlayerMoneyCollectController : SingletonBehaviour<PlayerMoneyCollectController>
    {
        protected override void OnAwake()
        { }

        public int CurrencyAmount { get; set; }


    }
}