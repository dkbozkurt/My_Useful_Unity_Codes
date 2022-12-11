// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Custom_Attributes.NameAndActionAttribute.Scripts
{
    /// <summary>
    ///  Ref : https://www.geeksforgeeks.org/custom-attributes-in-c-sharp/
    /// </summary>
    public class NameAndActionAttributeTest : MonoBehaviour
    {
        private void Start()
        {
            NameAndActionAttribute.AttributeDisplay(typeof(Employer));
  
            Debug.Log("\n");
  
            NameAndActionAttribute.AttributeDisplay(typeof(Employee));
        }
    }
}
