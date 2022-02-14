using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _text;

    private bool _tutorialEnded=false;
    void Start()
    {
        _hand.SetActive(true);
        _text.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tutorialEnded && Input.GetMouseButtonDown(0))
        {
            _hand.SetActive(false);
            _text.SetActive(false);
            _tutorialEnded = true;
        }
    }
}
