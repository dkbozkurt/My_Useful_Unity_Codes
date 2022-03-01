// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Import to use list of "Task" !!! 
using DG.Tweening; // Do not forget to import !!!
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

/// <summary>
/// Attach this script into an empty GameObject
///
/// Do not forget to import "using DG.Tweening;"
///
/// Ref : https://www.youtube.com/watch?v=Y8cv-rF5j6c&t=760s
///
/// DoTween asset : https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
/// </summary>

public class DoTweenSequencesTasks : MonoBehaviour
{
    [SerializeField] private float _cycleLength = 2f;
    [SerializeField] private GameObject[] _shapes=null;
    private void Start()
    {
        //_Sequence();
        //AsyncSequence();
        //Tasks();
    }

    private void _Sequence()
    {
        // DoTweem.Sequence(), can be used to avoid complex nested .OnCompletes.
        var sequence = DOTween.Sequence();

        foreach (var shape in _shapes)
        {
            sequence.Append(shape.transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f)));
            // !!! We can add sequence.Join(x), too, this time last appended and joins x operation will work at the same time
            // sequence.Insert(t,x) x operation will work after t time of the sequences start point.
            // .AppendCallBack(() => x) will call the added x operation at its queue.
        }

        sequence.OnComplete(() =>
        {
            foreach (var shape in _shapes)
            {
                shape.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            }
        });
    }

    // async type method's working logic is similar with Ienumerator's. Here we are combining await with .AsyncWaitForCompletion()
    // so it is simply works as OnComplete().
    private async void AsyncSequence()
    {
        foreach (var shape in _shapes)
        {
            await shape.transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f))
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion();
        }
    }

    // 
    private async void Tasks()
    {
        var tasks = new List<Task>();

        foreach (var shape in _shapes)
        {
            tasks.Add(shape.transform.DOMoveX(10, UnityEngine.Random.Range(1f, 2f))
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion());
        }

        await Task.WhenAll(tasks);

        foreach (var shape in _shapes) shape.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
    }
    
}