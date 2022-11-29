//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;
using DKBozkurt.Utils;

public class UtilsTest : MonoBehaviour
{
    [SerializeField] private Transform _testObject;
    private void Start()
    {
        DKBozkurtUtils.Events.OnScoreUpdated.Invoke(1);

        //StartCoroutine(NonAllocatingWaitTest());
        
    }

    private void Update()
    {
        // _testObject.transform.position =DKBozkurtUtils.GetMouseWorldPositionWithDistance(10);
        _testObject.transform.position =DKBozkurtUtils.GetMousePositionByCreatingPlaneOnZAxis();
    }

    private void OnEnable()
    {
        DKBozkurtUtils.Events.OnScoreUpdated.AddListener(TestCustomScoreEventMethod);
    }

    private void OnDisable()
    {
        DKBozkurtUtils.Events.OnScoreUpdated.RemoveListener(TestCustomScoreEventMethod);
    }

    private void TestCustomScoreEventMethod(int score)
    {
        Debug.Log($"score : {score}");
    }
    
    private IEnumerator NonAllocatingWaitTest()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return DKBozkurtUtils.GetWait(5f);
            Debug.Log("Dogukan");
        }
    }
}
