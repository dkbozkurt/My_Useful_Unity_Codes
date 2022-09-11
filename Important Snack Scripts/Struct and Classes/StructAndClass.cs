// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Struct_and_Classes
{
    /// <summary>
    /// Struck and Class
    /// 
    /// Structs are Value type. It's info hold's in stack. Stack : It will be removed when it's work done.
    ///
    /// Class - Reference type,It's info hold's in heap. Heap : It will need to remove by user.
    ///
    /// Note : Struct's elemenets can not be changed when it is in List.
    /// Ref : https://www.youtube.com/watch?v=NzlPn_6Knb0
    /// </summary>

    public class StructAndClass : MonoBehaviour
    {
        public List<CharacterInfoStruct> characterInfoStruct;
        public List<CharacterInfoClass> characterInfoClass;
        public CharacterInfoStruct characterInfoStructOutList;

        private void Start()
        {
            // Class
            characterInfoClass[0].name = "Kaan";
            Debug.Log(characterInfoClass[0].name);
            
            // Struct
            characterInfoStructOutList.name = "Kaan";
            Debug.Log(characterInfoStructOutList.name);
            
            // Gives an error because it cant not be changeable when it is in List
            // characterInfoStruct[0].name = "Kaan";
            // Debug.Log(characterInfoStruct[0].name);

        }
    }
    
    [Serializable]
    public struct CharacterInfoStruct
    {
        public string name;
        public int age;
    }
    
    [Serializable]
    public class CharacterInfoClass
    {
        public string name;
        public int age;
    }
}
