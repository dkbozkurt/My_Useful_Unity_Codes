// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace Generics.IntermediateGenericsTarodev
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=YEHbjy3JBtE
    /// </summary>
    
    public class HeroHelper<T> where T : Hero
    {
        public T Data;
        
        public HeroHelper(T data)
        {
            Data = data;
        }

        public void Print()
        {
            Debug.Log(Data);
        }

        public void ForceHeroToAttack()
        {
            Data.Attack();
        }
    }

    //
    
    public abstract class Hero : IPing
    {
        public string Name;
        public int Damage;

        public void Attack()
        {
            Debug.Log($"{Name} did {Damage} damage.");
        }

        public void PingMap()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Mage : Hero
    {
        // Const
        public Mage(int damage)
        {
            
        }
    }

    public class Warrior : Hero
    {
        
    }

    public class Archer : Hero
    {
        
    }
    
    //

    public class Anvil : IPing
    {
        public void PingMap()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Mailbox : IPing
    {
        public void PingMap()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IPing
    {
        void PingMap();
    }

    public class GenericsIntDeclarations : MonoBehaviour { }
}
