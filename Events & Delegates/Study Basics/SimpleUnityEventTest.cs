// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Important to import !!!

/// <summary>
/// - - - UnityEvent - - - 
///
/// UnityEvent’in event’e göre en büyük artısı: kendisine illa koddan değil,
/// Inspector’dan da fonksiyon ekleyebilirsiniz. Ama bunun yanında önemli
/// bir eksisi de vardır: System.Func gibi delegate’lerin aksine,
/// UnityEvent’ler void harici bir şey döndürmeyi desteklemezler.
///
/// 1- UnityEvent’lerle çalışırken script’inizin başına “using UnityEngine.Events;” eklemelisiniz.
/// 2- UnityEvent tanımlaması, UnityEvent class’ından türeyen ve System.Serializable attribute‘üne
/// sahip yeni bir sınıf oluşturarak yapılır.
/// 3- “+=“in karşılığı AddListener, “-=“in karşılığı RemoveListener‘dır.
/// 4- UnityEvent’e atalı fonksiyonlar Invoke ile çağrılır.
///
/// Bu noktalara dikkat ederseniz, Inspector’dan da üzerine fonksiyon eklemesi yapabildiğiniz
/// event’lere sahip olacaksınız
/// 
/// </summary>

public class SimpleUnityEventTest : MonoBehaviour
{
    // Delegate tanimlamasi
    public delegate void NumberDelegate( int num1 );
 
    // UnityEvent tanımlaması
    [System.Serializable] public class NumberUnityEvent : UnityEvent<int> { }
 
    // Delegate objesi
    public NumberDelegate delegateObj;
 
    // UnityEvent objesi
    public NumberUnityEvent unityEventObj;
 
    private void Start()
    {
        // Fonksiyon eklemek
        delegateObj += NumberTest1;
        unityEventObj.AddListener( NumberTest1 );
 
        // Fonksiyonları çağırmak
        if( delegateObj != null )
            delegateObj( 5 );
 
        if( unityEventObj != null )
            unityEventObj.Invoke( 5 );
 
        // Fonksiyon çıkarmak
        delegateObj -= NumberTest1;
        unityEventObj.RemoveListener( NumberTest1 );
 
        // Tüm fonksiyonları çıkarmak
        delegateObj = null;
        unityEventObj.RemoveAllListeners();
    }
 
    private void NumberTest1( int value )
    {
        Debug.Log( value );
    }
}
