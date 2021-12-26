//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/*Script that contains the necessary functions to assign to 
 * the buttons in the menu scene. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //Import while working on scenes

public class MainMenu : MonoBehaviour
{
    //Necessary functions for the buttons to open the desired games
    /*public void CompleteOurBody()
    {
        SceneManager.LoadScene("Game1");
    }*/
    public void MatchingOrganandSystem()
    {
        SceneManager.LoadScene("Game");
    }
    /*
    public void Options()
    {
        
    }
    */
}
