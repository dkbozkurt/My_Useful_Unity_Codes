// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Game.Scripts.Generics
{
    /// <summary>
    /// Intro to Generics
    /// 
    /// Ref : https://www.youtube.com/watch?v=7VlykMssZzk&t=6s
    /// </summary>

    public class BasicGenerics : MonoBehaviour
    {
        private delegate void MyActionDelegate<T1, T2>(T1 t1, T2 t2);
        private Action<int,string> action;

        private delegate T3 MyFuncDelegate<T1, T3>(T1 t1);
        public Func<int,bool> func;
        
        private void Start()
        {
            int[] intArray = CreateArray<int>(5, 6);
            Debug.Log(intArray.Length + " " + intArray[0] + " " + intArray[1]);
            Debug.Log(intArray.GetType());

            
            string[] stringArray = CreateArray<string>("asdf", "qwerty");
            Debug.Log(stringArray.GetType());
            
            
            MultiGenerics<int,string>(11,"dogukan");

            
            TestClass<int> testClass = new TestClass<int>();

            TestClassWithInterface<EnemyMinion> testClassWithInterfaceMinion = new TestClassWithInterface<EnemyMinion>(new EnemyMinion());
            TestClassWithInterface<EnemyArcher> testClassWithInterfaceArcher = new TestClassWithInterface<EnemyArcher>(new EnemyArcher());
            
        }

        private T[] CreateArray<T>(T firstElement, T secondElement)
        {
            return new T[] {firstElement, secondElement};
        }

        private void MultiGenerics<T1, T2>(T1 t1,T2 t2)
        {
            Debug.Log(t1.GetType());
            Debug.Log(t2.GetType());
        }
    
    }

    public class TestClass<T>
    {
        public T value;
        
        // We dont have to use <T> after than the method name, it auto defines if its in a generic class.
        private T[] CreateArray(T firstElement, T secondElement)
        {
            return new T[] {firstElement, secondElement};
        }
    }
    
    public class TestClassWithInterface<T> where T: IEnemy
    {
        public T value;

        public TestClassWithInterface(T value)
        {
            value.Damage();
        }

        // We dont have to use <T> after than the method name, it auto defines if its in a generic class.
        private T[] CreateArray(T firstElement, T secondElement)
        {
            return new T[] {firstElement, secondElement};
        }
    }

    
    
    public interface IEnemy
    {
        void Damage();
    }

    public class EnemyMinion : IEnemy
    {
        public void Damage()
        {
            Debug.Log("EnemyMinion.Damage()");
        }
    }
    
    public class EnemyArcher : IEnemy
    {
        public void Damage()
        {
            Debug.Log("EnemyArcher.Damage()");
        }
    }
    
    // Here what we are saying is, type T must be a class, it must implement IVehicle, and it must have a parameter
    // unless constructor.
    public class MyClass<T> where T : class, IVehicle<int>, new()
    {
        
    }
    
    // We can also use generics with interfaces
    public interface IVehicle<T>
    {
        void Damage(T t);
    }

    public class Car : IVehicle<int>
    {
        public void Damage(int i)
        {
            Debug.Log("Car.Damage()");
        }
    }

    public class Truck : IVehicle<float>
    {
        public void Damage(float i)
        {
            Debug.Log("Truck.Damage()");
        }
    }
}
