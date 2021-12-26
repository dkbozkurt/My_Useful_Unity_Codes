//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/* Displaying name of the object in a certain place of the scene.
 * 
 * Put this on the object (which need a collider for this to work) and drag a Text 
 * object onto the slot in the inspector.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Import the library.
using TMPro;    //Import the library.

public class DisplayName : MonoBehaviour
{
    [SerializeField] private GameObject textObj;      //Give the tmp into inspector
    private TMP_Text textObject;

    //Accessing the TMP component.
    private void Awake()
    {
        textObject = textObj.GetComponent<TMP_Text>();
    }

    public void Start()
    {
        textObj.SetActive(false);
    }
    public void OnMouseOver()
    {
        textObj.SetActive(true);
        //Debug.Log("Name of the object is" + this.gameObject.name );
        textObject.text= gameObject.name;

    }
    public void OnMouseExit()
    {
        textObj.SetActive(false);
    }

}
