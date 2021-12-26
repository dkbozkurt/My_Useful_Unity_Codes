//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for rotating an object in the game.
 * Add script into object that you want to rotate.
 * And assign the functions for buttons from the inspector.
 * 
 * Hint: Add "Event Trigger" to buttons that will rotate 
 * then add Pointer Down and Pointer Up events and assign
 * required functions.
 */

public class RotateObject : MonoBehaviour
{
    [SerializeField] private GameObject rotateObject;

    private float rotateSpeed = 50f;
    private bool rotatestatus = false;
    private bool negrotatestatus = false;

    //In the positive direction
    public void ObjectRotate()
    {

        rotatestatus = true;       
    }

    //In the negative direction
    public void NegObjectRotate()
    {
        negrotatestatus = true;
                
    }
    //Stops the rotation if mouse is up.
    public void StopObjectRotate()
    {
        rotatestatus = false;
        negrotatestatus = false;

    }

    private void Update()
    {
        
        if (rotatestatus)
        {
            rotateObject.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
        if (negrotatestatus)
        {
            rotateObject.transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        

    }

    //If it is desired to run when the button is clicked, and to stop when it is clicked second time.
    /*
    //In the positive direction
    public void ObjectRotate()
    {
        
        if (!rotatestatus)
        {
            rotatestatus = true;
            negrotatestatus = false;
        }
        else
        {
            rotatestatus = false;
        }
        
    }

    //In the negative direction
    public void NegObjectRotate()
    {
        if(!negrotatestatus)
        {
            negrotatestatus = true;
            rotatestatus = false;
        }
        else
        {
            negrotatestatus = false;
        }
    }

    private void Update()
    {
        
        if (rotatestatus)
        {
            rotateObject.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);     
        }
        if (negrotatestatus)
        {    
            rotateObject.transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }


    }
    */
}