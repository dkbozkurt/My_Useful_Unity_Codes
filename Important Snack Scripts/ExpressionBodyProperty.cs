// Dogukan Kaan Bozkurt
//          github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ExpressionBodyProperty
///
/// If we are using single line method with return than we can use expression body property by help
/// of the lambda expression.
/// 
/// "{get { }}" == "=>"
/// 
/// Ref: https://www.youtube.com/watch?v=KRq0-0KY6bU&ab_channel=JasonWeimann
/// </summary>

public class ExpressionBodyProperty : MonoBehaviour
{
    private float _nextSpawnTime;
    
    // Instead of using that we can use the following type
    //
    // bool ReadyToSpawnGem
    // {
    //     get
    //     {
    //         return Time.time >= _nextSpawnTime;
    //     }
    // }
    
    // Expression Body Property

    bool ReadyToSpawnGem => Time.time >= _nextSpawnTime;
}
