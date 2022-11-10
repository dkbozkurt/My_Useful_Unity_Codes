// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugGizmos.Unity_Gizmos
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=4Osn0g_Y24k&ab_channel=Tarodev
    /// </summary>
    public class GridByGizmos : MonoBehaviour
    {
        [SerializeField] private GameObject _node;
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private int _width = 7;
        [SerializeField] private int _depth = 10;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            foreach (var point in EvaluateGridPoints())
            {
                Instantiate(_node, point, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            if(!_drawGizmos || Application.isPlaying) return;
            foreach (var point in EvaluateGridPoints())
            {
                Gizmos.DrawWireCube(point, new Vector3(1,0,1));
            }
        }

        private IEnumerable<Vector3> EvaluateGridPoints()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    yield return new Vector3(x, 0, z);
                }
            }
        }
    }
}
