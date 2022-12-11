using System;
using System.Collections;
using UnityEngine;

namespace CpiTemplate.Game.Playable.Scripts.PlayableConnections
{
    public class CtaController : MonoBehaviour
    {
        // [LunaPlaygroundField("Open store after seconds", 0, "Store Settings")] [SerializeField]
        // private float _openStoreAfterSeconds = 9999;
        //
        // [LunaPlaygroundField("Open store after taps", 1, "Store Settings")] [SerializeField]
        // private int _openStoreAfterTaps = 9999;
        //
        // private int _tapCounter;
        // private void Awake()
        // {
        //     DoAfterSeconds(_openStoreAfterSeconds, OpenStore);
        // }
        //
        // private void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         _tapCounter++;
        //
        //         if (_tapCounter >= _openStoreAfterTaps)
        //         {
        //             Luna.Unity.LifeCycle.GameEnded();
        //             OpenStore();
        //         }
        //     }
        // }
        //
        // public void OpenStore()
        // {
        //     Luna.Unity.Playable.InstallFullGame();
        // }
        //
        // public static void OpenStoreStatic()
        // {
        //     Luna.Unity.Playable.InstallFullGame();
        // }
        //
        // private void DoAfterSeconds(float seconds, Action action)
        // {
        //     StartCoroutine(Do());
        //
        //     IEnumerator Do()
        //     {
        //         yield return new WaitForSeconds(seconds);
        //         action?.Invoke();
        //     }
        //     
        // }
        //
        // public void OpenEndCardAfterTaps()
        // {
        //     EndCardController.Instance.OpenEndCard();
        // }
    }
}