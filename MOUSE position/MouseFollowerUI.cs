// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mouse follower UI elements
/// </summary>
public class MouseFollowerUI : MonoBehaviour
{
    private List<GameObject> mouseImages = new List<GameObject>();
    private int _currentImageIndex = 0;
    private bool _mouse = true;
    private Animator _animator;

    private void Start()
    {
        AssignMouseImages();
        CursorSettings();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _mouse = !_mouse;
            CursorSettings();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_currentImageIndex < mouseImages.Count-1) _currentImageIndex++;
            else
            {
                _currentImageIndex = 0;
            }
            ChangeCursorImage();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("clicked",true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("clicked",false);
        }

        FollowMouse();
    }

    private void AssignMouseImages()
    {
        foreach (Transform images in transform.GetChild(0))
        {
            mouseImages.Add(images.gameObject);
        }
    }

    private void CursorSettings()
    {
        Cursor.visible = _mouse;
        transform.GetChild(0).gameObject.SetActive(!_mouse);
    }

    private void ChangeCursorImage()
    {
        for (int i=0;i < mouseImages.Count;i++)
        {
            mouseImages[i].SetActive(false);
        }
        mouseImages[_currentImageIndex].SetActive(true);
    }

    private void FollowMouse()
    {
        transform.position = Input.mousePosition;
    }
}