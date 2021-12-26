//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/* Simple Drag and Drop for inventory systems by using UI.
 * For dropping area. ( Item Slot )
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Import for the following operations.
using UnityEngine.UI;   //Import to access ui components.

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform tpTarget;
    [SerializeField] private ScoreSystem scoreSys;
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private ScoreAnimControl animationControl;
    [SerializeField] private RandomOrgan randomOrg;
    
    //This function get called, when draggable objects is dropped in here.
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
  
        //Correct : If there is an object dropped into the slot and their tags are same.
        //if(eventData.pointerDrag != null ) can be used if the object dont need to be checked for the slot.
        if (eventData.pointerDrag.tag==this.tag)   
        {
            //When we dropped the item it will locate itself at the middle of the slot.
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Score system
            scoreSys.setScore(180);
            scoreSys.scoreText.text = "SKOR: " + scoreSys.getScore();
            scoreSys.setnumOfOrgans(1);

            //Animation and Sound scripts.
            soundHandler.PlayCorrect();
            animationControl.Check(1);

            //Checks if number of the organs enough to end the game.
            if (scoreSys.getnumOfOrgans() == 10)
            {
                scoreSys.GameEndScreen();
            }
            //Placed organ can not be controlled anymore. !!!
            eventData.pointerDrag.GetComponent<Image>().raycastTarget = false;

            //Placed organ set to true, so next random organ can appear in the game.
            randomOrg.placedOrgan = true;
            
        }
        else if (eventData.pointerDrag.tag != this.tag)
        {
            //Score system
            scoreSys.setScore(-90);
            scoreSys.scoreText.text = "SKOR: " + scoreSys.getScore();

            //Animation and Sound scripts.
            soundHandler.PlayWrong();
            animationControl.Check(2);
                
            //If the object displayed in a wrong place, it will teleport back to "tpTarget".
            eventData.pointerDrag.transform.position = tpTarget.transform.position;
        }
        
    }
}
