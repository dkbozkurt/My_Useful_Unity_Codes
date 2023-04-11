// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;

namespace RollingCube.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=V9rVZ9mf0uA&ab_channel=Tarodev
    /// </summary>
    public class RollCubeController : MonoBehaviour
    {
        [SerializeField] private float _rollSpeed = 3f;
        private bool _isMoving;

        private void Update()
        {
            if(_isMoving) return;
            
            if (Input.GetKey(KeyCode.A)) Assemble(Vector3.left);
            else if (Input.GetKey(KeyCode.D)) Assemble(Vector3.right);
            else if (Input.GetKey(KeyCode.W)) Assemble(Vector3.forward);
            else if (Input.GetKey(KeyCode.S)) Assemble(Vector3.back);

            void Assemble(Vector3 dir)
            {
                var anchor = transform.position + (Vector3.down + dir) * 0.5f;
                var axis = Vector3.Cross(Vector3.up, dir);
                StartCoroutine(Roll(anchor, axis));

            }
        }

        private IEnumerator Roll(Vector3 anchor,Vector3 axis)
        {
            _isMoving = true;

            for (int i = 0; i < (90 / _rollSpeed); i++)
            {
                transform.RotateAround(anchor,axis,_rollSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            
            _isMoving = false;
        }
    }
}
