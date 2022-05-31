using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Behaviours.UI;
using Game.Scripts.Controllers;
using UnityEngine;
using RocketUtils;
using Game.Scripts.Models;
using Game.Scripts.Views.Panels;

public class CreativeLevelController : MonoBehaviour
{
    [SerializeField] private InGamePanel _inGamePanel;
    [SerializeField] private RefreshBehaviour _refreshBehaviour;
    [SerializeField] private List<GameObject> _uiElements;
    private bool uiStatus;
    public bool ResetPlayerPrefs = false;

    private void Start()
    {
        ResetPlayerPrefs = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PreviousLevel();
        }

        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.P))
        {
            ClearPlayerPrefs();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            uiStatus = !uiStatus;
            CloseUI();
        }
        
        
    }

    private void NextLevel()
    {
        _inGamePanel.OnRewardedSkipClicked();
    }


    private void PreviousLevel()
    {
        int previousLevel = PlayerData.Instance.PlayerLevel - 2;
        if (previousLevel >= 0)
        {
            PlayerData.Instance.PlayerLevel = Convert.ToInt32(InGameController.Instance.Levels[previousLevel]);
        }

        ResetLevel();
    }

    private void ResetLevel()
    {
        _refreshBehaviour.OnRefreshButtonClick();
    }

    private void ClearPlayerPrefs()
    {
        ResetPlayerPrefs = true;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void CloseUI()
    {
        foreach (GameObject child in _uiElements)
        {
            child.SetActive(uiStatus);
        }   
    }
}