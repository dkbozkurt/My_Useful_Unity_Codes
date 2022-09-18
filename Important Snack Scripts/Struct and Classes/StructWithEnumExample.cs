using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Creative.Scripts.Alexa
{
    public enum AlexaCostumeName
    {
        Prison,
        Casual
    }
    
    [Serializable]
    public struct AlexaCostumeSet
    {
        //public string costumeIndex;
        public List<GameObject> costumes;
    }
    
    public class StructWithEnumExample
    {
        [HideInInspector] public int costumeSetProcessIndex = 0;
        
        [SerializeField] private AlexaCostumeName alexaCostumeName = AlexaCostumeName.Prison;
        [SerializeField] private List<AlexaCostumeSet> alexaCostumeSets = new List<AlexaCostumeSet>();
        private int _costumeSetIndex=0;
        
        private void Start()
        {
            DecideInitialCostume();
        }
        
        public void ContinueCostumeProgress()
        {
            CostumeSetter(alexaCostumeSets[_costumeSetIndex].costumes[costumeSetProcessIndex],false);
            
            if(costumeSetProcessIndex >= alexaCostumeSets[_costumeSetIndex].costumes.Count-1) return;
            
            costumeSetProcessIndex++;
            CostumeSetter(alexaCostumeSets[_costumeSetIndex].costumes[costumeSetProcessIndex],true);
        }

        private void DecideInitialCostume()
        {
            switch (alexaCostumeName)
            {
                case AlexaCostumeName.Prison:
                    _costumeSetIndex = 0;
                    break;
                
                case AlexaCostumeName.Casual:
                    _costumeSetIndex = 1;
                    break;
                default:
                    Debug.LogException(new NullReferenceException());
                    break;
            }

            CostumeSetter(alexaCostumeSets[_costumeSetIndex].costumes[0], true);

        }
        
        private void CostumeSetter(GameObject costume, bool status)
        {
            costume.SetActive(status);
        }
        
        
    }
}
