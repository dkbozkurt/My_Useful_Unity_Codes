// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   // Important to import !!!

/// <summary>
///
/// - - - Events - - - 
/// 
/// 1- Event objesi tanımlarken event kelimesi kullanılmalıdır.
///
/// 2- Event objelerine fonksiyonları sadece “+=” ile ekleyebilirsiniz,
/// “=” yapmaya kalkarsanız hata alırsınız. Delegate’lerdeki “-=” olayı event’lerde de bulunmaktadır.
///
/// 3- Event objesindeki fonksiyonları eventObjesi() şeklinde çağırma işlemini sadece event objesinin
/// tanımlandığı sınıfta yapabilirsiniz. Örneğin eventObjesi’ni A sınıfında tanımladıysanız,
/// A sınıfı harici bir sınıf eventObjesi(); kodunu çalıştıramaz.
/// 
/// </summary>

public class SimpleEventTest : MonoBehaviour
{
    // Assigning delegate
    public delegate void TestDelegate(int num);

    // Creating object of Delegate
    public TestDelegate delegateObj;
    
    // Creating object of event
    public event TestDelegate eventObj;
    
    
}
