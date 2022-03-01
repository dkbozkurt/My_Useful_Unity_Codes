// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Linq;
using DG.Tweening; // Do not forget to import !!!
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this script into an empty GameObject.
///
/// Do not forget to import "using DG.Tweening;"
///
/// Ref : https://www.youtube.com/watch?v=Y8cv-rF5j6c&t=760s
///
/// DoTween asset : https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
/// </summary>

public class DoTweenAdditionalFeatures : MonoBehaviour
{
    [SerializeField] private GameObject _jumper, _puncher, _shaker, _target, _changer;

    private void Start()
    {
        // Jump();
        // Shake();
        // Punch();
        // Change();
    }
    
    private void Jump()
    {
        _jumper.transform.DOJump(
            new Vector3(-2.2f, 1f, 0f),
            3,
            1,
            0.5f)
            .SetEase(Ease.InOutSine);
    }

    private void Shake()
    {
        const float duration = 0.5f;
        const float strength = 0.5f;

        // _shaker.transform.DOShakePosition(duration, strength);
        // _shaker.transform.DOShakeRotation(duration, strength);
        // _shaker.transform.DOShakeScale(duration, strength);
        
        
        // Here, if we continually call the "Shake" method, _shaker gameObject's transform will move so
        // we do stop calls if tween is still playing.
        var tween = _shaker.transform.DOShakePosition(duration, strength);
        
        if(tween.IsPlaying()) return;
        
        _shaker.transform.DOShakeRotation(duration, strength);
        _shaker.transform.DOShakeScale(duration, strength);
        
        // if we are about to destroy an object, and it has a tween running,
        // we use DoKill to stop the all tweens, before destroying the object.
        // Always kill twins before, destroying the object in case.
        _shaker.transform.DOKill();

    }

    private void Punch()
    {
        var duration = 0.5f;
        _puncher.transform.DOPunchPosition(
            Vector3.left * 2,
            duration,
            0,
            0);

        _target.transform.DOShakePosition(
            duration,
            0.5f,
            10).SetDelay(duration * 0.5f);

    }
    
    private void Change()
    {
        _changer.GetComponent<MeshRenderer>().material.DOColor(UnityEngine.Random.ColorHSV(), 0.3f).OnComplete(Change);
    }
}