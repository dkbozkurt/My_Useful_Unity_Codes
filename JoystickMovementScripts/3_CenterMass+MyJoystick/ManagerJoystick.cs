// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Type 3

/// <summary>
/// Attach this script to "JoystickBackGround" object which should be prepared by you.(Basic Joystick)
/// 
/// "imgJoystickBg" should refer to background of joystick. Should be parent object and this script must add as a component.
/// "imgJoystickCenter" should refer to center dot of the jotstick and should be child object of "imgJoystickBg"
/// 
/// </summary>

public class ManagerJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    private Image imgJoystickBg;
    private Image imgJoystickCenter;
    private Vector2 posInput;

    // Start is called before the first frame update
    void Start()
    {
        imgJoystickBg = gameObject.GetComponent<Image>();
        imgJoystickCenter = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgJoystickBg.rectTransform, 
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
        {
            posInput.x = posInput.x / (imgJoystickBg.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imgJoystickBg.rectTransform.sizeDelta.y);
            // Debug.Log(posInput.x.ToString()+'/'+posInput.y.ToString());
            
            //normalize

            if (posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }
            
            //joystick move
            imgJoystickCenter.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (imgJoystickBg.rectTransform.sizeDelta.x/2),
                posInput.y * (imgJoystickBg.rectTransform.sizeDelta.y/2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imgJoystickCenter.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float inputHorizontal()
    {
        if (posInput.x != 0)
        {
            return posInput.x;
        }
        else
            return Input.GetAxis("Horizontal");
    }

    public float inputVertical()
    {
        if (posInput.y != 0)
        {
            return posInput.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
    
}
