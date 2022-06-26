using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _text;
    //[SerializeField] private GameObject _arrow;

    private bool _tutorialEnded=false;
    void Start()
    {
        _hand.SetActive(true);
        _text.SetActive(true);
        //_arrow.SetActive(true);
    }
    
    void Update()
    {
        if (!_tutorialEnded && Input.GetMouseButtonDown(0))
        {
            _hand.SetActive(false);
            _text.SetActive(false);
            //CloseArrowAfterTime();
            _tutorialEnded = true;
        }
    }

    private void CloseAfterTime(float time, GameObject objectToClose)
    {
        StartCoroutine(Do());
        IEnumerator Do()
        {
            yield return new WaitForSeconds(time);
            objectToClose.SetActive(false);
        }
    }
}
