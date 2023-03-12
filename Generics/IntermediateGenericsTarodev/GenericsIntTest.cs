// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Generics.IntermediateGenericsTarodev
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=YEHbjy3JBtE
    /// </summary>

    public class GenericsIntTest : MonoBehaviour
    {
        public void Start()
        {
            TestGeneric();
        }

        public void TestGeneric()
        {
            var warrior = new Warrior()
            {
                Name = "Dogukan",
                Damage = 4
            };

            var helper = new HeroHelper<Warrior>(warrior);
            helper.Print();
        }

        public void TestHeroFactory()
        {
            // Here 'new()' is avoiding create heroes with constructor.
            T HeroFactory<T>(string heroName) where T : Hero, new()
            {
                T newHero = new T();
                newHero.Name = heroName;
                return newHero;
            }

            // So here we can use Archer but cant use Mage class.
            var archer = HeroFactory<Archer>("Legolas");
            // var mage = HeroFactory<Mage>("Gandalf");

        }

        public void TestGenericWithInterface()
        {
            void PingMap<T>(T inputObject) where T : IPing
            {
                inputObject.PingMap();
            }
        }
        
    }
    
    
}
