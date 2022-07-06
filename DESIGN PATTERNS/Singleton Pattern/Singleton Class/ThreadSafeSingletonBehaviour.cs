// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.DesignPatterns
{
    
    /// <summary>
    /// Thread-safe Singleton Design Pattern
    ///
    /// Lets you ensure that a class has only one instance, while providing a global access point to this instance.
    /// In other words, it is an architecture that is introduced to create only one object from a class.
    ///
    /// ---
    /// All implementations of the Singleton have these two steps in common:
    ///
    /// 1- Make the default constructor private, to prevent other objects from using the new operator with the Singleton class.
    /// 2- Create a static creation method that acts as a constructor. Under the hood, this method calls the private constructor
    /// to create an object and saves it in a static field. All following calls to this method return the cached object.
    ///
    /// If your code has access to the Singleton class, then it’s able to call the Singleton’s static method. So whenever that method is called, the same object is always returned.
    /// ---
    ///
    /// -Note- [IN CASE OF REMOVING SINGLETON FROM A CLASS] : If we need to remove Singleton from Inheritance we can call the Instance func in the current class.
    ///
    /// for example: public static ClassName Instance => FindObjectOfType<ClassName>();
    /// Here ClassName is the Class that we removed SingletonBehaviour<T> 
    ///
    /// 
    /// Note: volatile: Instead of pulling the value of the variable from the data register, it pulls it from the memory, this takes more time, but returns more accurate results.
    /// </summary>
    
    // Thread-safe Singleton
    public abstract class ThreadSafeSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        // volatile: Instead of pulling the value of the variable from the data register, it pulls it from the memory, this takes more time, but returns more accurate results.
        // private static volatile T _instance = null;

        private static T _instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(T)) as T; // same as (T)FindObjectOfType(typeof(T));
                if (Instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                                   " is needed in the scene, but there is none.");
                }
                else
                {
                    OnAwake();
                }
            }
            else if (Instance != this)
            {
                // On reload, singleton already set, so destroy duplicate.
                Destroy(gameObject); // prevent duplicates
            }
        }

        protected abstract void OnAwake();
        
        public static T Instance
        {
            get
            {
                if (!_instance) // means _instance == null
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (!_instance)
                    {
                        Debug.LogError("An instance of "+ typeof(T) +
                                  " is needed in the scene, but there is none.");
                    }
                }
                else
                {
                    ThreadSafeSingletonBehaviour<T> instance = _instance as ThreadSafeSingletonBehaviour<T>;
                    instance.OnAwake();
                }

                return _instance;
            }
            
            set
            {
                _instance=value;
            }
        }
    }
}