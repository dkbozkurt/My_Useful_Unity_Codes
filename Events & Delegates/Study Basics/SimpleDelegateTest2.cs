// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

/// <summary>
///
/// - - - Delegate 2 - - -
///  
/// Delegate is a container for a function that can be used as a variable.
/// Delegates are used to perform tasks assigned to different methods with one call.
///
/// Note: Return type of the func. and input parameter types should be same with the delegate type's.
///
/// System.Action‘da “<” ve “>” işaretleri (generic) arasına, delegate’in aldığı parametreleri yazabilirsiniz.
/// System.Action delegate’ler void döndürürler. System.Func delegate’ler ise void harici bir şey döndürürler
/// ve bu döndürdükleri türü, “<” ve “>” işaretlerinin son parametresi olarak alırlar; ondan önceki parametreler
/// ise, fonksiyonun aldığı parametreleri belirtir.
///
/// public delegate void SayiDelegate( int sayi );
/// public delegate int SayiDelegate2( string yazi );
///      
/// //public SayiDelegate delegateObjesi; // SayiDelegate ile
/// public System.Action<int> delegateObjesi; // System.Action ile
///  
/// //public SayiDelegate2 delegateObjesi2; // SayiDelegate2 ile
/// public System.Func<string, int> delegateObjesi2; // System.Func ile
/// 
/// </summary>

public class SimpleDelegateTest2 : MonoBehaviour
{
    // Assigning delegate
    public delegate int DelegateTest (string str0);

    // Creating object of Delegate
    public DelegateTest delegateObj;

    private void Start()
    {
        Test();
    }

    private void Test()
    {
        delegateObj += NumberTest1;
        delegateObj += NumberTest2;

        if (delegateObj != null)
        {
            // Note: All delegate type objects are an object of System.Delegate Class.
            System.Delegate[] functions = delegateObj.GetInvocationList();

            for (int i = 0; i < functions.Length; i++)
            {
                //int result = ( (SayiDelegate2) functions[i] ).Invoke( "Test" );       // !!! typecast not working
                //Debug.Log( "result: " + result);
            }
        }
    }

    private int NumberTest1(string str1)
    {
        return str1.Length;
    }

    // Functions can be static.
    public static int NumberTest2(string str2)
    {
        return -1;
    }
}
