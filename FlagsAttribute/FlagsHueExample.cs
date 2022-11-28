// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace FlagsAttribute
{
    /// <summary>
    /// 
    /// Ref : https://learn.microsoft.com/tr-tr/dotnet/api/system.flagsattribute?view=net-6.0
    /// </summary>
    public enum SingleHue
    {
        None = 0,
        Black = 1,
        Red = 2,
        Green = 4,
        Blue = 8
    };
        
    [System.Flags]
    enum MultiHue : short
    {
        // None = 0,
        // Black = 1,
        // Red = 2,
        // Green = 4,
        // Blue = 8
        
        // Can be written as [FAV]
        None = 0,
        Black = 1 << 0,
        Red = 1 << 1,
        Green = 1 << 2,
        Blue = 1 << 3
        
        // Also can be written as
        // None = 0x0,
        // Black = 0x1,
        // Red = 0x2,
        // Green = 0x4,
        // Blue = 0x8
        
    };
    
    public class FlagsHueExample : MonoBehaviour
    {
        
        private void Awake()
        {
            // Display all possible combinations of values.
            Debug.Log(
                "All possible combinations of values without FlagsAttribute:");
            for(int val = 0; val <= 16; val++ )
                Debug.LogFormat( "{0,3} - {1:G}", val, (SingleHue)val);

            // Display all combinations of values, and invalid values.
            Debug.Log(
                "\nAll possible combinations of values with FlagsAttribute:");
            for( int val = 0; val <= 16; val++ )
                Debug.LogFormat( "{0,3} - {1:G}", val, (MultiHue)val);
        }
        
    }
    
    
}
