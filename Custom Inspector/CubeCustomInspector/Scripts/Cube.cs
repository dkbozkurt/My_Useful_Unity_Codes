// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// USED TOGETHER WITH CUSTOM INSPECTOR SCRIPT
/// Scripts name "CubeEditor"
/// </summary>
public class Cube : MonoBehaviour
{
   [HideInInspector] public float baseSize = 1f;
    
    private void Start()
    {
        GenerateColor();
    }
    
    private void Update()
    {
        float animation = baseSize + Mathf.Sin(Time.time * 8f) * baseSize / 7f;
        transform.localScale = Vector3.one * animation; 
    }
    
    public void GenerateColor()
    {
        GetComponent<Renderer>().sharedMaterial.color = Random.ColorHSV();
    }
    
    public void Reset()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.white;
        
    }
    
}
