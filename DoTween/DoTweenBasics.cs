// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Linq;
using DG.Tweening; // Do not forget to import !!!
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this script into an empty GameObject, that dotween operations will be applied.
///
/// Do not forget to import "using DG.Tweening;"
///
/// Ref : https://www.youtube.com/watch?v=Y8cv-rF5j6c&t=760s
///
/// DoTween asset : https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
/// </summary>

public class DoTweenBasics : MonoBehaviour
{
    [SerializeField] private float _cycleLength = 2f;
    [SerializeField] private GameObject childObject = null;
    [SerializeField] private GameObject[] _shapes = null;
    private void Start()
    {
        //_Move();
        //_Ease();
        //_Loops();
        //_Rotate();
        //_localMove();
        //_Scale();
        //_OnComplete();
        //_Virtual();
        //_SpeedBased();
    }

    private void _Move()
    {
        // .DoMove(target,t), moves the object to the target value. in a t time.
        // .DoMoveX/.DoMoveY/.DoMoveZ do some progress target needs to get 1 float value.
        transform.DOMove(new Vector3(10, 0, 0), _cycleLength);
        
        // if we use DoMove(target).From(); then, it will execute from the entered target position to start position
    }

    private void _Ease()
    {
        // .SetEase(e), helps to control animation curve of the movement. e: ease type
        transform.DOMove(new Vector3(10, 0, 0), _cycleLength)
            .SetEase(Ease.InBounce);
    }

    private void _Loops()
    {
        // .SetLoops(n,type), sets the looping operation for the tween. n: number of cycles, type: loop type
        transform.DOMove(new Vector3(10, 0, 0), _cycleLength)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1,LoopType.Yoyo);
    }

    private void _Rotate()
    {
        // .DoRotate(target,t,r) rotates transform rotation value, to target value in t time. r: Help us to select, rotation mode.(rotates beyond 360 in here)
        transform.DORotate(new Vector3(0, 360, 0),_cycleLength,RotateMode.FastBeyond360)
            .SetLoops(-1,LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void _localMove()
    {
        // If you have child object and want to move it, respect to its parent object.
        childObject.transform.DOLocalMove(new Vector3(0, -3, 0), _cycleLength * 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

    }

    private void _Scale()
    {
        // .DoScale(target,t) scales transform to the target value in t time.
        transform.DOScale(Vector3.zero, _cycleLength * 0.5f)
            .SetEase(Ease.InBounce);
    }
    
    private void _OnComplete()
    {
        // .OnComplete(() => {x}) calls the x action when tween is completed.
        _shapes[0].transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f)).OnComplete(() => {
            _shapes[1].transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f)).OnComplete(() => {
                _shapes[2].transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f)).OnComplete(() =>
                {
                    foreach (var shape in _shapes)
                    {
                        shape.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);

                    }
                });
            });
        });
    }

    private void _Virtual()
    {
        // .DoVirtual(a,b,t,v) creates virtual tweens that can be used to change other elements via their onUpdate calls (v)
        // a: start point, b: end point, t: time, v: onUpdate call value
        
        DOVirtual.Float(0,10,3,v =>
        {
            print(v);
        }).SetEase(Ease.InBounce).SetLoops(-1,LoopType.Yoyo);
    }

    private void _SpeedBased()
    {
        // .SetSpeedBased(b), if b is true, sets the tween as speed based (duration will represent the number of
        // units the tween moves x second).  
        transform.DOMoveX(10, _cycleLength).SetSpeedBased(true);

    }
    
}
