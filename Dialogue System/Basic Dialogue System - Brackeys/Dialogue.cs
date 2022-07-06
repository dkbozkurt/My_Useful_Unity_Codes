// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Dialogue_System.Basic_Dialogue_System___Brackeys
{
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=_nRzoTzeyxU 
    /// </summary>
    
    [Serializable]
    public class Dialogue
    {
        public string name;
        
        [TextArea(3,10)]
        public string[] sentences;

    }
}