// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using Assembly_Definitions.Test;
using UnityEngine;

namespace AssemblyDefinitions.Core.Assembly_Definitions.Core
{
    /// <summary>
    /// Without adding Assembly Definition References you wont be able to access
    /// Test interface.
    /// Ref : https://www.youtube.com/watch?v=HYqOSkHI674&ab_channel=InfallibleCode
    /// </summary>
    public class AssemblyDefinitionsCore : MonoBehaviour, IAssemblyDefinitionsTestInterface
    {
        public void MethodForAssemblyDefinitions()
        {
            Debug.Log("Hi");
        }
    }
}