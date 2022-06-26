using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class ColorChanger : MonoBehaviour
    {
        public enum PlayerUniformType
        {
            Uniform,
            Plain
        }
        #region Game Variables

        [SerializeField] private List<GameObject> players;
        [SerializeField] private Material plainMat;
        [SerializeField] private List<GameObject> playerCapObjects;

        #endregion
        
        
        #region LunaPlaygroundFields

        [LunaPlaygroundField("Players Uniform Type", 0, "Color Settings")] [SerializeField]
        private PlayerUniformType uniformType;

        [LunaPlaygroundField("Player's material Color", 1, "Color Settings")] [SerializeField]
        private Color materialColor;
        

        #endregion
        
        private void Awake()
        {
            plainMat.color = materialColor;
            DecideUniformType();
            ChangeHeadObjects();
        }

        private void DecideUniformType()
        {
            switch (uniformType)
            {
                case PlayerUniformType.Uniform:

                    foreach (GameObject eachPlayer in players)
                    {
                        SkinnedMeshRenderer renderer = eachPlayer.GetComponentInChildren<SkinnedMeshRenderer>();
                        Material[] mats = renderer.materials;
                        mats[0].color = plainMat.color;
                        renderer.materials = mats;
                    }
                    break;
                
                case PlayerUniformType.Plain:

                    foreach (GameObject eachPlayer  in players)
                    {
                        SkinnedMeshRenderer renderer = eachPlayer.GetComponentInChildren<SkinnedMeshRenderer>();
                        Material[] mats = renderer.materials;
                        mats[0] = plainMat;
                        renderer.materials = mats;
                    }
                    break;
            }
        }
        
        private void ChangeHeadObjects()
        {
            foreach (GameObject child in playerCapObjects)
            {
                child.SetActive(uniformType == PlayerUniformType.Uniform );
            }
        }
    }
}