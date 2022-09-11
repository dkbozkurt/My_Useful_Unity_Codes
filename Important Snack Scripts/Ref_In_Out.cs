// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace ImportantSnackScripts.Scripts
{
    /// <summary>
    /// Ref & In & Out
    /// 
    /// Ref : https://www.youtube.com/watch?v=LFo7OakOxEg
    /// </summary>

    public class Ref_In_Out : MonoBehaviour
    {
    
        private int _intValue = 5;

        private void Start()
        {
            // Ref();
            // ut();
            // In();
        }

        #region Ref

        private void Ref()
        {
            // Without Using 'ref' it will give 5;
            SquareOperation(_intValue);
            Debug.Log(_intValue);
            
            // With using 'ref' it will give 25;
            SquareOperationWithRef(ref _intValue);
            Debug.Log(_intValue);

        }

        private void SquareOperation(int value)
        {
            value *= value;
        }

        private void SquareOperationWithRef(ref int value)
        {
            value *= value;
        }

        #endregion

        #region Out

        private void Out()
        {
            ReturnMultipleOutputs(_intValue ,out string nameOfAuthor, out int ageOfAuthor, out bool isAuthorWorking);
            Debug.Log($"Name : {nameOfAuthor}");
            Debug.Log($"Age : {ageOfAuthor}");
            Debug.Log($"Working : {isAuthorWorking}");
        }

        private void ReturnMultipleOutputs(int value, out string name, out int age, out bool isWorking)
        {
            Debug.Log($"Value :{value}");
            
            // 3 outputs
            name = "Dogukan Kaan Bozkurt";
            age = 24;
            isWorking = true;
        }
        

        #endregion
        
        #region In

        // 'in' modifier make input read-only so we cant change the value;
        private void In()
        {
            InValue(_intValue);
        }

        private void InValue(in int value)
        {
            // Gives an error
            // value += 5;
            
            Debug.Log("Value didnt change with in modifier : "+ value);
        }

        #endregion
    }
}
