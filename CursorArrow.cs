//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Script to change cursor image while picking and at the stand position.
 */

public class CursorArrow : MonoBehaviour
{
    public Texture2D cursorArrow;
    public Texture2D cursorPick;

    //Default cursor.
    void Start()
    {
        //It hides the cursor.
        //Cursor.visible = false;
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    void OnMouseEnter()
    {   
        Cursor.SetCursor(cursorPick, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    private void OnMouseExit()
    {
        /*
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorPick, Vector2.zero, CursorMode.ForceSoftware);
        }
        */
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

}
