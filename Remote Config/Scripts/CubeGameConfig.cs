// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig; // Add namespace for remove config !!!

/// <summary>
/// REMOTE CONFIG
///
/// Attach this script into the gameObject that gonna be effected by the remote configs. 
/// </summary>

public class CubeGameConfig : MonoBehaviour
{
    // Created 2 struct variable
    public struct userAttributes { }
    public struct appAttributes { }

    
    public bool isBlue = true;

    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        ConfigManager.FetchCompleted += SetColor;
        // In order to call a function we call FetchConfigs.
        // It goes to the server, and pulls down all of the settings that are there.
        // It is default syntax.
        ConfigManager.FetchConfigs<userAttributes,appAttributes>(new userAttributes(),new appAttributes());
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void SetColor(ConfigResponse response)
    {
        isBlue = ConfigManager.appConfig.GetBool("cubeIsBlue");
        CubeMaterialStatus();
    }

    private void CubeMaterialStatus()
    {
        if (isBlue) _renderer.material = blueMaterial;
        else _renderer.material = redMaterial;

    }

    private void Update()
    {
        // It basically pulls the changes into the game when clicked.
        if (Input.GetMouseButtonDown(0))
        {
            ConfigManager.FetchConfigs<userAttributes,appAttributes>(new userAttributes(),new appAttributes());
        }
    }

    private void OnDestroy()
    {
        ConfigManager.FetchCompleted -= SetColor;
    }
}

