// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Animation_Rigging.MakingAnimationsWithoutAnimations
{
    /// <summary>
    /// Animation Rigging
    ///
    /// !!! YOU MUST WATCH THE VIDEO TO APPLY SCRIPT INTO GAME !!!
    /// !!! YOU MUST IMPORT "ANIMATION RIGGING" PACKAGE FROM PACKAGE MANAGER !!!
    /// 
    /// Ref : https://www.youtube.com/watch?v=UL2EbxqwozM
    /// </summary>

    public class GrabAnimWithAnimationRigging : MonoBehaviour
    {
        [SerializeField] private Transform handIKTarget;
        [SerializeField] private Transform handBone;
        
        [SerializeField] private float grabRange = 1.5f;
        private ItemGrabbable _itemGrabbable = null;
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            DetectItemsNearby();

            if (Input.GetKeyDown(KeyCode.E))
            {
                GrabItem();
            }
        }

        private void DetectItemsNearby()
        {
            // Detects Object around grabRange.
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, grabRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.gameObject.TryGetComponent<ItemGrabbable>(out _itemGrabbable))
                {
                    _itemGrabbable.ShowIcon();
                    break;
                }
            }
        }
        private void GrabItem()
        {
            if(_itemGrabbable == null) return;

            handIKTarget.position = _itemGrabbable.transform.position;
            _animator.SetTrigger("GrabItem");
            // _itemGrabbable.DestroySelf();
        }

        // Call from animation as event.
        private void OnAnimationGrabbedItem()
        {
            _itemGrabbable.transform.SetParent(handBone,true);
            
        }
        
        // Call from animation as event.
        private void OnAnimationStoredItem()
        {
            _itemGrabbable.DestroySelf();
        }
        
    }
}
