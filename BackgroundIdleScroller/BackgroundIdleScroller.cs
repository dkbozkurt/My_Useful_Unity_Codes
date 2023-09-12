using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=-6H-uYh80vc&ab_channel=Tarodev
/// </summary>
public class BackgroundIdleScroller : MonoBehaviour
{
    [SerializeField] private RawImage _backgroundRawImage;
    [SerializeField] private float _x = 0.01f;
    [SerializeField] private float _y = 0.01f;

    private void Update()
    {
        AnimateBackgroundImage();
    }

    private void AnimateBackgroundImage()
    {
        _backgroundRawImage.uvRect = new Rect(_backgroundRawImage.uvRect.position 
                                              + new Vector2(_x, _y) * Time.deltaTime, _backgroundRawImage.uvRect.size);
    }
}
