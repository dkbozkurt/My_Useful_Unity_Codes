// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

namespace Game.Scripts.DesignPatterns
{
    /// <summary>
    /// Naïve Singleton Design Pattern
    ///
    /// Lets you ensure that a class has only one instance, while providing a global access point to this instance.
    /// In other words, it is an architecture that is introduced to create only one object from a class.
    ///
    /// Ref: https://www.youtube.com/watch?v=1Uv6IEv7E2Y&t=942s&ab_channel=Emirhan%C5%9Eenkal
    /// </summary>

    // Toy Singleton
    public class SimpleSingleton : MonoBehaviour
    {
        private static SimpleSingleton _instance = null;

        public static SimpleSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SimpleSingleton>();
                    if(_instance == null)
                        _instance = new GameObject("SimpleSingleton").AddComponent<SimpleSingleton>();
                }

                return _instance;
            }
        
        }

    }
}

