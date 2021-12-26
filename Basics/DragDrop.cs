//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/* Simple Drag and Drop for inventory systems by using UI.
 * This script for draggable objects ( Items ) 
 * + This script includes simple image changer functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //Import for the following operations.
using UnityEngine.UI;   //Import for the ui element operations.

//In the class, we implement various classes to control if mouse drag or not etc.
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler , IEndDragHandler ,IDragHandler 
{
    //Functions must be public because, we inherited another class.

    //OutOfSlot message object.
    [SerializeField] private GameObject outOfSlot;
    //Location for spawning objects
    [SerializeField] private Transform tpTarget;
    //RandomOrgan script imported.
    [SerializeField] private RandomOrgan randomOrg;
    //ScoreAnimControl script imported.
    [SerializeField] private ScoreAnimControl animationControl;

    private RectTransform rectTransform;

    //Images to change depends on the situations.
    [SerializeField] private Sprite pos0;    //When object appeared in the game.
    [SerializeField] private Sprite pos1;    //When object is dragging.
    [SerializeField] private Sprite pos2;    //When object placed in the correct slot.

    [SerializeField] private GameObject organSlotParent;

    private GameObject correctOrganCanvas;

    //Currect image of the object.
    private Image currentImg;

    //Canvas
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        //Object's rectTransfrom assigned.
        rectTransform = GetComponent<RectTransform>();
        //Object's canvasgroup assigned.
        canvasGroup = GetComponent<CanvasGroup>();

        //Object's picture assigned as default
        currentImg = GetComponent<Image>();
        currentImg.GetComponent<Image>().sprite = pos0;
    }

    //This function get called, we begin dragging.
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        //To make transparent the object while dragging.
        canvasGroup.alpha = .6f;

        //Closed the raycast Target option during dragging !!!
        canvasGroup.blocksRaycasts = false;

        //When object is dragged, pos1 will be displaying.
        currentImg.GetComponent<Image>().sprite = pos1;
    }

    //This function get called on every frame while we are dragging the object.
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        //To drag the object with mouse's movement. !!!
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //This function get called, we stop dragging.
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");

        //To make visible the object when dragging ended.
        canvasGroup.alpha = 1f;

        //Opened the raycast Target option when object dropped!!!
        canvasGroup.blocksRaycasts = true;

        //Teleports the organ back to tpTarget position if didn't located in a slot !!!
        if (!randomOrg.placedOrgan)
        {
            //Display the outofSlot message.
            outOfSlot.SetActive(true);

            //Teleport the organ to tpTarget location
            eventData.pointerDrag.transform.position = tpTarget.transform.position;

            currentImg.GetComponent<Image>().sprite = pos0;
        }

        //If the object placed correctly.
        else
        {
            currentImg.GetComponent<Image>().sprite = pos2;
            
            //if the object placed correctly, find the correct canvas object
            correctOrganCanvas = FindObjectInChildWithTag(organSlotParent, gameObject.tag);
            //set the correct organ as a child of correct organ canvas
            this.transform.SetParent(correctOrganCanvas.transform);
            
            //set the correct organ's size as correct organ canvas's size respectively
            RectTransform parent = correctOrganCanvas.transform.GetComponent<RectTransform>();
            GridLayoutGroup grid = correctOrganCanvas.transform.GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(parent.rect.width, parent.rect.height);
        }
    }
    
    //find the correct organ canvas and return it
    private GameObject FindObjectInChildWithTag(GameObject parent, string tag){
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }

    //This function get called, we clicked on the object
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPrinterDown");

        //Score animation set as default.
        animationControl.Check(0);

        //When clicked on the organ, hide the message.
        outOfSlot.SetActive(false);

    }
}
