// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Static Handmade Joystick
///
/// Attach this script onto "JoystickBackground" gameobject that has "JoystickHande" as a child gameobject. 
/// 
/// Send return values of inputHorizontal and inputVertical methods into expected place. 
/// </summary>

public class StaticJoystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    private Image imgJoystickBg;
    private Image imgJoystickCenter;
    private Vector2 posInput;
    
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
