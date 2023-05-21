// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace PerlinNoise.Random_Value_With_Perlin_Noise.Scripts
{
    public class RandomValueWithPerlinNoise : MonoBehaviour
    {
        [SerializeField]private int _width=20;
        [SerializeField]private int _depth=20;
        [SerializeField]private GameObject _objectPrefab;

        [Header("Perlin Noise Properties")]
        [SerializeField] private float _perlinNoiseAmplitude = 0.2f;
        [SerializeField] private float _perlinNoiseMultiplier = 3f;

        private void Start()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    var positionY = Mathf.PerlinNoise(x * _perlinNoiseAmplitude, z * _perlinNoiseAmplitude) *
                                    _perlinNoiseMultiplier;

                    Vector3 pos = new Vector3(x, positionY, z);
                    GameObject go = Instantiate(_objectPrefab, pos, Quaternion.identity);
                }
            }
        }
        
    }
}
