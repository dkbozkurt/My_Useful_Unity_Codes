using PlayableAdsKit.Scripts.Base;
using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class ObjectPool : KitButtonBase
    {
        ObjectPool()
        {
            Name = "Object Pool Manager";
            DescriptionText = "Imports Object Pool Design Pattern to help optimization and performance improvement";
        }

        protected override void RunAction()
        {
            GameObject playableObjectPool;
            if (FindObjectOfType<ObjectPoolManager>())
            {
                Debug.LogWarning("There is already a ObjectPoolManager exist in the scene!");
                return;
            }
            if (GameObject.Find("ObjectPoolManager") != null)
            {
                playableObjectPool = GameObject.Find("ObjectPoolManager");
            
                if (playableObjectPool.TryGetComponent(out ObjectPoolManager objectPool))
                {
                    return;
                }
                else
                {
                    playableObjectPool.AddComponent<ObjectPoolManager>();
                    playableObjectPool.transform.position = Vector3.zero;
                }
                return;
            }

            playableObjectPool = new GameObject("ObjectPoolManager");
            playableObjectPool.AddComponent<ObjectPoolManager>();
            playableObjectPool.transform.position = Vector3.zero;
            Debug.Log("Object Game Manager successfully instantiated!");
        }
    }
}
