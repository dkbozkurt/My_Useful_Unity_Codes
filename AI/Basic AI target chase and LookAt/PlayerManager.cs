// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Attach this script onto game manager object.
    /// Ref : https://www.youtube.com/watch?v=xppompv1DBg&list=RDCMUCYbK_tjZ2OrIZFBvU6CCMiA&start_radio=1
    /// </summary>
    
    public class PlayerManager : MonoBehaviour
    {
        #region Simple Singleton

        public static PlayerManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [HideInInspector] public GameObject player;
    }
}