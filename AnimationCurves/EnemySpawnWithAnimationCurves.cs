// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace AnimationCurves
{
    /// <summary>
    /// Animation Curves
    /// Ref : https://www.youtube.com/watch?v=roWiGo1Hpfk
    /// </summary>

    public class EnemySpawnWithAnimationCurves : MonoBehaviour
    {
        [SerializeField] private AnimationCurve enemySpawnAmountCurve;
        [SerializeField] private AnimationCurve enemyHealthAmountCurve;

        [SerializeField] private float eachWaveSpawnDuration=6f;
        private int _waveIndex;
        private float _nextWaveTimer;
        private void Update()
        {
            _nextWaveTimer -= Time.deltaTime;
            if (_nextWaveTimer <= 0f)
            {
                _nextWaveTimer = eachWaveSpawnDuration;
                SpawnWave();
            }

        }

        private void SpawnWave()
        {
            _waveIndex++;
            int spawnEnemyAmount = Mathf.RoundToInt(enemySpawnAmountCurve.Evaluate(_waveIndex));
            Debug.Log("Spawned enemy amount: " + spawnEnemyAmount);
            
            for (int i = 0; i < spawnEnemyAmount; i++)
            {
                SpawnEnemy(Mathf.RoundToInt(enemyHealthAmountCurve.Evaluate(_waveIndex)));
            }
        }

        private void SpawnEnemy(int healthAmount)
        {
            Debug.Log("Spawned enemy heath amount: " + healthAmount);
        }
    }
}
