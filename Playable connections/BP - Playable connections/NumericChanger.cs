using System;
using Game.Scripts.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class NumericChanger : MonoBehaviour
    {
    
    [LunaPlaygroundField("Open end card after balloon pop", 0, "Numerical Settings")] [SerializeField]
    private int _openStoreAfterBalloonPop;

    [SerializeField] private LevelBehaviour _levelBehaviour;
    [SerializeField] private CtaController _ctaController;
    [SerializeField] private EndCardController endCardController;
    [SerializeField] private GameObject ball;

    private int numberOfBallons;

    private bool directed;
    
    private void Awake()
    {
        numberOfBallons = _openStoreAfterBalloonPop;
        directed = false;
    }

    private void Update()
    {
        if ( !directed && _levelBehaviour._poppedBalloonCount >= numberOfBallons)
        {
            //_ctaController.OpenStore();
            endCardController.Enable();
            ball.GetComponent<Rigidbody>().isKinematic = true;
            directed = true;
        }
    }
    }
}