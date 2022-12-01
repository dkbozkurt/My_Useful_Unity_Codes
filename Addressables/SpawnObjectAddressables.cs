// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Addressables
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid)
        {
        }
    }

    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=C6i_JiRoIfk&ab_channel=CodeMonkey
    /// </summary>
    public class SpawnObjectAddressables : MonoBehaviour
    {
        [SerializeField] private string _addressablePath; // "Assets/Addressables/Prefabs/Environment.prefab"

        // It helps us to use addressables without string path.
        [SerializeField] private AssetReference _assetReference;

        // It helps us to use addressables labes.
        [SerializeField] private AssetLabelReference _assetLabelReference;
        
        // Specific type of AssetReference
        [SerializeField] private AssetReferenceGameObject _assetReferenceGameObject;
        
        // Custom created specific type of AssetReference
        // [SerializeField] private AssetReferenceAudioClip _assetReferenceAudioClip;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                //LoadAddressable(_addressablePath);
                //LoadAddressable2(_addressablePath);
                //LoadAddressable3(_assetReference);
                //LoadAddressable4(_assetLabelReference);
                //CompactLoadAddressable(_assetReferenceGameObject);
                //LoadMultipleAddressable(_assetLabelReference);

            }
        }

        /// <summary>
        /// Addressable Load with seperated function with string input.
        /// </summary>
        #region Type_1

        private void LoadAddressable1(string addressablePath)
        {
            AsyncOperationHandle<GameObject> asyncOperationHandle = 
                UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(addressablePath);

            asyncOperationHandle.Completed += AsyncOperationHandle_Completed;

        }

        private void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(asyncOperationHandle.Result);
            }
            else
            {
                Debug.LogError("Failed to load!");
            }
        }

        #endregion

        /// <summary>
        /// Addressable Load with delegate function with string input.
        /// </summary>
        #region Type2

        private void LoadAddressable2(string addressablePath)
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(addressablePath).Completed +=
                (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Instantiate(asyncOperationHandle.Result);
                    }
                    else
                    {
                        Debug.LogError("Failed to load!");
                    }
                };
        }

        #endregion

        /// <summary>
        /// Addressable Load with delegate function with assetReferance.
        /// </summary>
        #region Type3
        private void LoadAddressable3(AssetReference assetReference)
        {
            assetReference.LoadAssetAsync<GameObject>().Completed +=
                (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Instantiate(asyncOperationHandle.Result);
                    }
                    else
                    {
                        Debug.LogError("Failed to load!");
                    }
                };
        }
        

        #endregion
        
        /// <summary>
        /// Addressable Load with delegate function with tags.
        /// </summary>
        #region Type4

        private void LoadAddressable4(AssetLabelReference assetLabelReference)
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(assetLabelReference).Completed +=
                (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Instantiate(asyncOperationHandle.Result);
                    }
                    else
                    {
                        Debug.LogError("Failed to load!");
                    }
                };
        }

        #endregion
        
        /// <summary>
        /// Compact Addressable Load.
        /// </summary>
        #region CompactType

        private void CompactLoadAddressable(AssetReferenceGameObject assetReferenceGameObject)
        {
            assetReferenceGameObject.InstantiateAsync();
        }

        #endregion

        /// <summary>
        /// Load multiple addressables with Labels.
        /// </summary>
        #region LoadMultipleAddressables

        private void LoadMultipleAddressable(AssetLabelReference assetLabelReference)
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<Sprite>(assetLabelReference, (sprite) =>
            {
                Debug.Log(sprite);
            });
        }

        #endregion

        /// <summary>
        /// Unload addressable can be used with game object label or path.
        /// BE SURE THAT BEFORE RELEASE YOUR LOADING PROCESS MUST COMPLETED.
        /// </summary>
        #region OnloadAddressables

        private void UnLoadAddressable()
        {
            // Can be used to release addressable
            // UnityEngine.AddressableAssets.Addressables.Release(_addressablePath);
            
            // Can be used with instantiated/ loaded gameobject.
            // UnityEngine.AddressableAssets.Addressables.ReleaseInstance("Loaded game objects comes here!");
        }

        #endregion
    }
}
