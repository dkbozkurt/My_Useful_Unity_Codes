// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


namespace AI.NavMesh
{
    /// <summary>
    ///
    /// Unity NavMesh Tutorial - Basics
    ///
    ///  Attach this script onto Agent.
    /// 
    /// We will click some part of level and player will automatically move to that location,
    /// We will look for mouse input and will send out a ray to the point where the player pressed,
    /// will then look for where this ray hit and will get the position of that point.
    /// 
    /// Ref : https://www.youtube.com/watch?v=CHV1ymlw-P8&list=RDCMUCYbK_tjZ2OrIZFBvU6CCMiA&index=3
    /// </summary>

    [RequireComponent(typeof(NavMeshAgent))]    
    public class AIClickMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MoveAgent();
            }
        }

        private void MoveAgent()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Physics.Raycast return bool so if ray hits something then we want to do operation.
            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }
}
