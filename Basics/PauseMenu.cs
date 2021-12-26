//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/*
 * The script for the menu page and information window.
 * also animation connections through buttons
 * add script into canvas and adjust the scenes from the inspector.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //For pause menu
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseButton;

    //For rotate button
    [SerializeField] private GameObject rotateButton;
    [SerializeField] private GameObject negrotateButton;

    //For the sub-window
    [SerializeField] private GameObject firstPlayButton;
    [SerializeField] private GameObject subWindowUI;
    [SerializeField] private GameObject systemsTable;

    //Animation
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private GameObject humanBody;

    private void Awake()
    {
        systemsTable.SetActive(false); 
        firstPlayButton.SetActive(true);
        subWindowUI.SetActive(true);

        rotateButton.SetActive(false);
        negrotateButton.SetActive(false);
        pauseButton.SetActive(false);

        humanBody.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
                
        rotateButton.SetActive(true);
        negrotateButton.SetActive(true);
        //Line for play the game in normal speed.
        //Time.timeScale = 1f;

        systemsTable.SetActive(true);
        animationManager.whiteBoardZoom(false);
        animationManager.HumanBodyInOut(true);
        animationManager.DoctorInOut(true);

        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        rotateButton.SetActive(false);
        negrotateButton.SetActive(false);
        //Line for stop the game.
        //Time.timeScale = 0f;
 
        systemsTable.SetActive(false);
        animationManager.whiteBoardZoom(true);
        animationManager.HumanBodyInOut(false);
        animationManager.DoctorSpriteController(0);
        animationManager.DoctorInOut(false);

        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void playAfterSubWindow()
    {
        subWindowUI.SetActive(false);
        firstPlayButton.SetActive(false);
        systemsTable.SetActive(true);
        
        rotateButton.SetActive(true);
        negrotateButton.SetActive(true);
        pauseButton.SetActive(true);

        humanBody.SetActive(true);
        animationManager.DoctorSpriteController(0);
        //Animations will be played
        animationManager.whiteBoardZoom(false);
        animationManager.HumanBodyInOut(true);
        animationManager.DoctorInOut(true);
        

    }


    /*
    //Info message functions
    public void InfoMessage()
    {
        if (!infoswitch)     //it helps us to make switch type of button.
        {
            infoMenuUI.SetActive(true);
            infoswitch = true;
        }
        else
        {
            infoMenuUI.SetActive(false);
            infoswitch = false;
        }     
    }
    */
  
    //PlayAgain function for Game2
    public void PlayAgain2()
    {
        SceneManager.LoadScene("Game");      
    }

    

    //We dont have quit button in our game so it is deactivated.
    /*
    public void QuitGame()
    {
        Application.Quit();
    }
    */
}
