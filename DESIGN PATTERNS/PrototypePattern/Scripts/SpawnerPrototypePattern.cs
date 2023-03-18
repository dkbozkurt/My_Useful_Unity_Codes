// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DESIGN_PATTERNS.PrototypePattern.Scripts
{
    /// <summary>
    /// 
    /// </summary>

    public class SpawnerPrototypePattern : MonoBehaviour
    {
        [SerializeField] private GameObject _cubePrefab;
        [SerializeField] private GameObject _spherePrefab;
        
        private void Update()
        {
            // InstantiateCubes();
            
            // InstantiateProcCube();
            InstantiateProcSphere();
        }
        
        private void InstantiateCubes()
        {
            if (Random.Range(0, 100) < 10)
                Instantiate(_cubePrefab, this.transform.position, quaternion.identity);
            else if(Random.Range(0,100) < 10)
                Instantiate(_spherePrefab, this.transform.position, quaternion.identity);
        }
        
        private void InstantiateProcCube()
        {
            if (Random.Range(0, 100) < 10)
            {
                ProcCube.Clone(this.transform.position);
            }
        }

        private void InstantiateProcSphere()
        {
            if (Random.Range(0, 100) < 10)
            {
                ProcSphere.Clone(this.transform.position);
            }
        }
        
    }
}
