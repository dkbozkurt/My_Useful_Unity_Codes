// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    
    /// <summary>
    /// Attach this script onto Enemy.
    /// 
    /// Ref : https://www.youtube.com/watch?v=xppompv1DBg&list=RDCMUCYbK_tjZ2OrIZFBvU6CCMiA&start_radio=1
    /// </summary>
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private float lookRadius = 10f;

        private Transform _target;
        private NavMeshAgent _agent;
        
        private void Start()
        {
            _target = PlayerManager.Instance.player.transform;
            _agent = GetComponent<NavMeshAgent>();

        }

        private void Update()
        {
            Chase();
        }

        private void Chase()
        {
            float distance = Vector3.Distance(_target.position, transform.position);

            if (distance <= lookRadius)
            {
                _agent.SetDestination(_target.position);

                if (distance <= _agent.stoppingDistance)
                {
                    // Attack the target
                    FocusTarget();
                }
            }
        }

        private void FocusTarget()
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime * 5f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,lookRadius); 
        }
    }
}
