// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>
public class ClickInputReader : MonoBehaviour
{
    public Vector3? GetClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo)) return hitInfo.point;
            
        }

        return null;
    }
}
