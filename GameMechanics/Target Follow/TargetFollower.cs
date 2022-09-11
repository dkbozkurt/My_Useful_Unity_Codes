// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Target_Follow
{
    /// <summary>
    /// Attach this script to the target that will be followed.
    /// 
    /// Ref : https://www.youtube.com/watch?v=LOHyIzKisrI
    /// </summary>

    public class TargetFollower : MonoBehaviour
    {

        [SerializeField] private Transform follower;
        [SerializeField] private float maxDistance = 2f;
        [SerializeField] private float speed = 1f;

        private void Update()
        {
            Follow();
        }

        private void Follow()
        {
            float actualDistance = Vector3.Distance(transform.position, follower.position);

            if (actualDistance > maxDistance)
            {
                var followToCurrent = (transform.position - follower.position).normalized;
                followToCurrent.Scale(new Vector3(maxDistance,maxDistance,maxDistance));

                transform.position = follower.position + followToCurrent;
                
                // Rotate
                // Determine which direction to rotate towards
                Vector3 targetDirection = follower.position - transform.position;
                
                // The step size is equal to speed times frame time.
                float singleStep = speed * Time.deltaTime;
                
                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                
                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
