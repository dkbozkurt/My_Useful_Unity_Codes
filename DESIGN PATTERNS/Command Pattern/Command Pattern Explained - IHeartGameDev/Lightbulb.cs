// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;
using Random = UnityEngine.Random;

namespace DESIGN_PATTERNS.Command_Pattern.Command_Pattern_Explained___IHeartGameDev
{
    /// <summary>
    ///
    /// COMMAND PATTERN
    ///
    /// Attach this script on to the lightbulb object that gonna turn on/off the light.
    ///
    /// Ref : https://www.youtube.com/watch?v=oLRINAn0cuw
    /// </summary>

    // RECEIVER
    public class Lightbulb : MonoBehaviour
    {
        // current lightbulb state
        private bool _isPowerOn = false;
        
        // this logic turn the light on/off
        public void TogglePower()
        {
            if (!_isPowerOn)
            {
                GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                transform.GetChild(0).gameObject.SetActive(true);
                _isPowerOn = true;
            }
            else
            {
                GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                transform.GetChild(0).gameObject.SetActive(true);  
                _isPowerOn = false;
            }
            
        }

        public void SetLightColor(Color newColor)
        {
            Material material = GetComponent<Renderer>().material;
            material.color = newColor;
            material.SetColor("_EmissionColor",newColor);
            transform.GetChild(0).gameObject.GetComponent<Light>().color = newColor;
        }

        public void SetRandomLightColor()
        {
            Color randonColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            Material material = GetComponent<Renderer>().material;
            material.color = randonColor;
            material.SetColor("_EmissionColor",randonColor);
            transform.GetChild(0).gameObject.GetComponent<Light>().color = randonColor;
        }
        



    }
}
