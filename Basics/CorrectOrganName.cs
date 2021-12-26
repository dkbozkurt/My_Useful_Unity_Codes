//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/*Displaying name of the object at a location if the object put in the correct collider.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrectOrganName : MonoBehaviour
{
    [SerializeField] private GameObject textObj;
    private TMP_Text textOrgan;

    private void Awake()
    {
        textOrgan = textObj.GetComponent<TMP_Text>();
    }

    public void Start()
    {
        textObj.SetActive(false);
        //textObj.transform.position = new Vector3(1000,1000,1000);
    }

    //Function for dispaying name of the object depends on the tag at the location.
    public void DisplayCorrectName(GameObject organ,string sysTag)
    {
        textObj.SetActive(false);
        
        switch (sysTag)
        {
            case "System1Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1226,891,0);
                textOrgan.text = "+" + organ.name;    
                break;
            case "System2Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1600, 870, 0);
                textOrgan.text = "+" + organ.name;
                break;
            case "System3Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1230, 555, 0);
                textOrgan.text = "+" + organ.name;
                break;
            case "System4Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1594, 546, 0);
                textOrgan.text = "+" + organ.name;
                break;
            case "System5Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1221, 219, 0);
                textOrgan.text = "+" + organ.name;
                break;
            case "System6Org":
                textObj.SetActive(true);
                textObj.transform.position = new Vector3(1592, 219, 0);
                textOrgan.text = "+" + organ.name;
                break;
            default:
                textObj.SetActive(false);
                break;

        }
        //If we want to show in a current location.
        //textObj.SetActive(true);
        //textOrgan.text = "Son doðru bilinen \n+" + organ.name;
    }

}
