using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayableRenderingSettings : MonoBehaviour
{
    public enum BallType
    {
        Blade,
        CaptainAmerica,
        Pokemon,
        DogeCoin
    }
    
    [LunaPlaygroundField("Ball to select", 0, "Ball Settings")] [SerializeField]
    private BallType _selectedBall;
    
    private BallType _ballType;
    [SerializeField] private GameObject[] blades;
    private void Awake()
    {
        _ballType = _selectedBall;
        DecideBallAndBalloons();
    }

    private void DecideBallAndBalloons()
    {
        switch (_ballType)
        {
            case BallType.Blade:
                ChangeBallType(0);
                break;
            
            case BallType.CaptainAmerica:
                ChangeBallType(1);
                break;
            
            case BallType.Pokemon:
                ChangeBallType(2);
                break;
            
            case BallType.DogeCoin:
                ChangeBallType(3);
                break;
        }
        
    }

    private void ChangeBallType(int _index)
    {
        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].SetActive(false);
        }
        blades[_index].SetActive(true);
    }
}
