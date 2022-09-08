using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenKilling : MonoBehaviour
{
    private Tween myTween;
    private void Start()
    {
       
        DoMoveSphere();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BreakTheTween();
        }
    }

    private void DoMoveSphere()
    {
        myTween = transform.DOMoveX(5, 1f).SetLoops(-1,LoopType.Yoyo);
    }

    private void BreakTheTween()
    {
        myTween.Kill();
    }
}
