// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace FlagsAttribute
{
    /// <summary>
    /// My own Example
    /// 
    /// Ref : https://stackoverflow.com/questions/56621287/check-if-an-enum-flag-contains-a-certain-flag-value
    /// </summary>

    [System.Flags]
    public enum SushiType
    {
        None = 0,
        Rice = 1 << 1,
        Nori= 1 << 2,
        Salmon= 1 << 3,
        Avocado= 1 << 4,
        Perch= 1 << 5,
        Crab= 1 << 6
    }
    
    
    public class FlagsSushiExample : MonoBehaviour
    {
        // Enum declaration.
        public SushiType DesiredSushiComponents;
        
        private SushiType _sushi1;
        private SushiType _sushi2;
        private SushiType _sushi3;
        private SushiType _sushi4;

        private SushiType[] _sushiSet;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            //ExpectedSushi(SushiType.Nori,SushiType.Rice);
            //ExpectedSushi(DesiredSushiComponents);
            //CheckType1();
            CheckType2();
            //AddAndRemove();
        }

        private void ExpectedSushi(params SushiType[] list)
        {
            Debug.Log("I want to have sushi with\n");

            for (int i = 0; i < list.Length; i++)
            {
                Debug.Log(list[i]+ " ");
            }
            
            Debug.Log("\n");

            for (int i = 0; i < _sushiSet.Length; i++)
            {
                for (int j = 0; j < list.Length; j++)
                {
                    CheckSushiSet(_sushiSet[i],list[j],i);    
                }
            }
        }

        private void CheckSushiSet(SushiType sushi, SushiType desiredSushiComponent,int sushiNumber)
        {
            sushiNumber++;
            if (sushi.HasFlag(desiredSushiComponent))
            {
                Debug.Log("Sushi number "+ sushiNumber + " has "+ desiredSushiComponent +" in it.");
            }
            
            // Same as 
            // if ((_sushiSet[i] & SushiType.Salmon) != 0)
            // {
            //     Debug.Log("Sushi number "+ i+1 + " has salmon in it.");
            // }
        }

        private void Initialize()
        {
            _sushi1 = SushiType.Rice | SushiType.Nori | SushiType.Perch;
            _sushi2 = SushiType.Rice | SushiType.Nori | SushiType.Crab | SushiType.Avocado;
            _sushi3 = SushiType.Rice | SushiType.Salmon;
            _sushi4 = SushiType.Rice | SushiType.Nori | SushiType.Avocado;

            _sushiSet = new SushiType[] {_sushi1, _sushi2, _sushi3, _sushi4};
        }

        private void CheckType()
        {
            SushiType sushi = SushiType.Rice | SushiType.Nori | SushiType.Avocado;
            if ((sushi & SushiType.Nori) == SushiType.Nori && (sushi & SushiType.Avocado) == SushiType.Avocado)
            {
                Debug.Log("Item has Nori and Avocado in it.");
            }
        }

        private void CheckType2()
        {
            SushiType sushi = SushiType.Rice | SushiType.Nori | SushiType.Avocado;
            if (sushi.HasFlag(SushiType.Nori | SushiType.Avocado))
            {
                Debug.Log("Item has Nori and Avocado in it.");
            }
        }

        private void AddAndRemove()
        {
            SushiType sushi = SushiType.Rice | SushiType.Nori | SushiType.Avocado;

            sushi = sushi | SushiType.Crab; // sushi |= SushiType.Crab;
            sushi = sushi & SushiType.Nori | SushiType.Perch; // sushi &= SushiType.Nori; and so on...
            
            Debug.Log("Sushi has crab in it: "+sushi.HasFlag(SushiType.Crab));
            Debug.Log("Sushi has nori in it: "+sushi.HasFlag(SushiType.Nori));
        }
    }
    
}
