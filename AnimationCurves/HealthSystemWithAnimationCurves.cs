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

    public class HealthSystemWithAnimationCurves : MonoBehaviour
    {
        [SerializeField] private AnimationCurve healthPerLevelAnimationCurve;

        public int PlayerLevel { set; get; }
        public int HealthAmount { set; get; }

        private void Awake()
        {
            PlayerLevel = 0;
            HealthAmount = Mathf.RoundToInt(healthPerLevelAnimationCurve.Evaluate(0));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerLevel++;
                Player_OnLevelUp();
            }
        }

        private void Player_OnLevelUp()
        {
            HealthAmount = Mathf.RoundToInt(healthPerLevelAnimationCurve.Evaluate(PlayerLevel));
            Debug.Log("playerLevel: " + PlayerLevel + " ; playerHealth: " + HealthAmount);
        }
    }
}
