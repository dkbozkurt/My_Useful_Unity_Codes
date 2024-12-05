using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpiShowcase : MonoBehaviour
{
    [Header("General Setup")]
    public GameObject[] showcaseObjects;
    public KeyCode nextKey = KeyCode.D;
    public KeyCode prevKey = KeyCode.A;

    [Header("Camera Rotation - Optional")]
    public bool useObjectRotation;
    public GameObject rotationObject;
    public float rotationSpeed;

    private int index = 0;

    void Update()
    {
        if(useObjectRotation) rotationObject.transform.Rotate(Vector3.up, rotationSpeed);

        if(Input.GetKeyDown(nextKey))
        {
            showcaseObjects[index].SetActive(false);

            index++;
            if (index >= showcaseObjects.Length) index = 0;

            showcaseObjects[index].SetActive(true);
        }

        if (Input.GetKeyDown(prevKey))
        {
            showcaseObjects[index].SetActive(false);

            index--;
            if (index < 0) index = showcaseObjects.Length - 1;

            showcaseObjects[index].SetActive(true);
        }
    }
}
