using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

/// <summary>
///
///
/// 
/// NOTE: This script must be used with "Paint in 3D" asset.
///       Used Throw object taken from scene number 31.
/// </summary>
public class PaintByThrowedObject : MonoBehaviour
{
    #region Throwed Object Components

    private GameObject _ball;
    private ParticleSystem _explosionColor;

    #endregion

    #region Changable Properties

    [SerializeField] private Mesh[] throwObjectMeshes;
    [SerializeField] private Material[] throwObjectMaterials;
    [SerializeField] private Texture[] throwObjectStains;

    
    #endregion

    #region Indexes

    private int meshIndex;
    private int objectMaterialIndex;
    private int stainIndex;

    private bool particleVisualization;

    #endregion
    
    private void Awake()
    {
        _ball = transform.GetChild(0).gameObject;
        _explosionColor = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

        AssignElementProperties();
    }

    void Update()
    {
        // To Change the mesh of the object
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (meshIndex < throwObjectMeshes.Length-1)
            {
                meshIndex++;
                ChangeObjectMesh();
            }
            else
            {
                meshIndex = 0;
                ChangeObjectMesh();
            }
            
        }
        
        // To Change the color of the object
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (objectMaterialIndex < throwObjectMaterials.Length-1)
            {
                objectMaterialIndex++;
                ChangeObjectMaterial();
            }
            else
            {
                objectMaterialIndex = 0;
                ChangeObjectMaterial();
            }
        }
        
        // To Change the stainTexture of the object
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (stainIndex < throwObjectStains.Length-1)
            {
                stainIndex++;
                ChangeStainTexture();
            }
            else
            {
                stainIndex = 0;
                ChangeStainTexture();
            }
        }
        
        // To change the particle statue of/off
        if (Input.GetKeyDown(KeyCode.F))
        {
             particleVisualization = !particleVisualization;
             ChangeParticleVisualization(particleVisualization);
        }
        
    }

    private void AssignElementProperties()
    {
        meshIndex = 0;
        objectMaterialIndex= 0;
        stainIndex = 0;
        particleVisualization = true;

        ChangeObjectMesh();
        ChangeObjectMaterial();
        ChangeStainTexture();
        ChangeParticleVisualization(particleVisualization);
    }

    #region Throwed Object Region
    private void ChangeObjectMesh()
    {
        _ball.GetComponent<MeshFilter>().mesh = throwObjectMeshes[meshIndex];
    }
    private void ChangeObjectMaterial()
    {
        _ball.GetComponent<MeshRenderer>().material = throwObjectMaterials[objectMaterialIndex];
        
        ChangeStainMaterial(throwObjectMaterials[objectMaterialIndex]);
        if(particleVisualization)
            ChangeParticleMaterial(throwObjectMaterials[objectMaterialIndex]);
    }
    #endregion

    #region Stain Region
    private void ChangeStainTexture()
    {
        _ball.GetComponent<P3dPaintDecal>().Texture = throwObjectStains[stainIndex];
        
    }
    private void ChangeStainMaterial(Material objectMat)
    {
        _ball.GetComponent<P3dPaintDecal>().Color = objectMat.color;
    }
    #endregion

    #region Particle Region
    private void ChangeParticleVisualization(bool status)
    {
        if (status)
            transform.GetChild(1).gameObject.GetComponent<P3dDestroyAfterTime>().Seconds = 1f;
        else
            transform.GetChild(1).gameObject.GetComponent<P3dDestroyAfterTime>().Seconds = 0f;

    }
    private void ChangeParticleMaterial(Material objectMat)
    {
        var main = _explosionColor.main;
        main.startColor = objectMat.color;
    }
    #endregion

    
}
