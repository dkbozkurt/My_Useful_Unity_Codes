// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - - - Lambda Expression - - -
///
/// lambda expression, kod içerisinde dinamik olarak isimsiz fonksiyonlar oluşturmaya yarar.
/// C# dünyasında lambda expression’lar, delegate’ler ile birlikte kullanılırlar.
///
/// Lambda expression’lar şu şekilde tanımlanır:
///
/// 1- Normal bir fonksiyon tanımlar gibi, parantezler açılır ve içerisine parametreler girilir.
/// 2- Lambda expression’lara has “=>” (lambda işleci) konulur.
/// 3- Süslü parantezler içerisine fonksiyonun kodu yazılır.
///
/// Lambda expression’ı değer olarak verdiğimiz delegateObjesi‘nin int parametre aldığı zaten
/// bilindiği için, parametrenin türünü lambda expression’da tekrardan yazmak zorunda değiliz.
/// 
/// </summary>
public class SimpleLambdaExpressionTest : MonoBehaviour
{
    public delegate void NumberDelegate(int num1);

    public NumberDelegate delegateObj;

    
    private void Start()
    {
        // LambdaExample("basicExample");
        LambdaExample("exampleWithUnsub");
    }

    private void LambdaExample(string status)
    {
        
        // We convert NumberTest1 func to lambda expression by using (int value) => {Debug.Log(value)};
        if (status == "basicExample")
        {
            delegateObj += (int value) =>
            {
                Debug.Log(value);
            };
            delegateObj(5);
        }
        
        // Lambda expression’larla ilgili çok dikkat edilmesi gereken bir nokta, “-=” kullanımıdır.
        // Lambda expression’lar bunu doğrudan desteklemez. Örneğin şu koddaki -= satırı bir işe yaramaz:
        // Önce lambda expression’ı bir delegate objesine (lambdaObjesi) değer olarak vermeli,
        // ardından artık bu delegate objesini “+=” ve “-=” ile kullanmalısınız.
        else if (status == "exampleWithUnsub")
        {
            NumberDelegate lambdaObj = ( value ) => Debug.Log( value );
            delegateObj += lambdaObj;
 
            delegateObj( 5 );
 
            delegateObj -= lambdaObj;
        }
        else
        {
            Debug.Log("Type valid property.");
        }
        
        
    }
    
    
    
}
