// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Access the functions either through code or buttons.
///
/// Add this script into an empty gameObject in the related scenes.
/// </summary>

public class UIController : MonoBehaviour
{
    public void ReloadScene()
    {
        Debug.Log("Game Scene is Reloading ...");
        Loader.Load(Loader.Scene.GameScene);
    }

    public void MainMenuScene()
    {
        Debug.Log("Main Menu is Loading ...");
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void LoadGameScene()
    {
        Debug.Log("Game Scene is Loading ...");
        Loader.Load(Loader.Scene.GameScene);
    }
}
