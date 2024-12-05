using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarNumerics : MonoBehaviour
{
    [LunaPlaygroundField("Speed multiplier", 0, "Car Physics Settings")] [SerializeField]
    private float speedMultiplier;

    [LunaPlaygroundField("Car Mass", 1, "Car Physics Settings")] [SerializeField]
    private float mass;

    [LunaPlaygroundField("Top Speed", 2, "Car Physics Settings")] [SerializeField]
    private float topSpeed;

    [LunaPlaygroundField("OpenParkArea after seconds", 3, "Car Physics Settings")] [SerializeField]
    private float parkAreaActivateSeconds;

    [SerializeField] private CarController _carController;
    [SerializeField] private CarUserControl _carUserControl;

    [SerializeField] private GameObject _parkAreaCollider;
    
 
    private void Awake()
    {
        _parkAreaCollider.SetActive(false);   
        
        _carUserControl._speedMultiplier = speedMultiplier;
        _carController.carMass = mass;
        _carController.m_Topspeed = topSpeed;
        
        WaitForPark(parkAreaActivateSeconds);
    }
    private void WaitForPark(float seconds)
    {
        StartCoroutine(Do());
        IEnumerator Do()
        {
            yield return new WaitForSeconds(seconds);
            _parkAreaCollider.SetActive(true);
        }
    }
}
