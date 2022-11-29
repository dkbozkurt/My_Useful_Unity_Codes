//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DKBozkurt.Utils
{
    public static partial class DKBozkurtUtils
    {
        /// <summary>
        /// Calls function on every Update until it returns true.
        /// </summary>
        public class FunctionCaller
        {
        private class MonoBehaviourHook : MonoBehaviour {

            public Action OnUpdate;

            private void Update() {
                if (OnUpdate != null) OnUpdate();
            }

        }

        private static List<FunctionCaller> updaterList; // Holds a reference to all active updaters
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change

        private static void InitIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionCaller_Global");
                updaterList = new List<FunctionCaller>();
            }
        }

        public static FunctionCaller Create(Action updateFunc) {
            return Create(() => { updateFunc(); return false; }, "", true, false);
        }

        public static FunctionCaller Create(Action updateFunc, string functionName) {
            return Create(() => { updateFunc(); return false; }, functionName, true, false);
        }

        public static FunctionCaller Create(Func<bool> updateFunc) {
            return Create(updateFunc, "", true, false);
        }

        public static FunctionCaller Create(Func<bool> updateFunc, string functionName) {
            return Create(updateFunc, functionName, true, false);
        }

        public static FunctionCaller Create(Func<bool> updateFunc, string functionName, bool active) {
            return Create(updateFunc, functionName, active, false);
        }

        public static FunctionCaller Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName) {
            InitIfNeeded();

            if (stopAllWithSameName) {
                StopAllUpdatersWithName(functionName);
            }

            GameObject gameObject = new GameObject("FunctionCaller Object " + functionName, typeof(MonoBehaviourHook));
            FunctionCaller functionCaller = new FunctionCaller(gameObject, updateFunc, functionName, active);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionCaller.Update;

            updaterList.Add(functionCaller);
            return functionCaller;
        }

        private static void RemoveUpdater(FunctionCaller functionCaller) {
            InitIfNeeded();
            updaterList.Remove(functionCaller);
        }

        public static void DestroyUpdater(FunctionCaller functionCaller) {
            InitIfNeeded();
            if (functionCaller != null) {
                functionCaller.DestroySelf();
            }
        }

        public static void StopUpdaterWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++) {
                if (updaterList[i].functionName == functionName) {
                    updaterList[i].DestroySelf();
                    return;
                }
            }
        }

        public static void StopAllUpdatersWithName(string functionName) {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++) {
                if (updaterList[i].functionName == functionName) {
                    updaterList[i].DestroySelf();
                    i--;
                }
            }
        }
        
        private GameObject gameObject;
        private string functionName;
        private bool active;
        private Func<bool> updateFunc; // Destroy Updater if return true;

        public FunctionCaller(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active) {
            this.gameObject = gameObject;
            this.updateFunc = updateFunc;
            this.functionName = functionName;
            this.active = active;
        }

        public void Pause() {
            active = false;
        }

        public void Resume() {
            active = true;
        }

        private void Update() {
            if (!active) return;
            if (updateFunc()) {
                DestroySelf();
            }
        }

        public void DestroySelf() {
            RemoveUpdater(this);
            if (gameObject != null) {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        }    
    }
}
