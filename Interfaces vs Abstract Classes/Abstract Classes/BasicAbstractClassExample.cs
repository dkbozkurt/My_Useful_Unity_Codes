//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Interfaces_vs_Abstract_Classes.Abstract_Classes
{
    /// <summary>
    /// Abstraction
    /// 
    /// Ref : https://www.youtube.com/watch?v=OoEFYMUPAV0
    /// </summary>
    
    public class BasicAbstractClassExample 
    {
        static void Main()
        {
            // Abstract class' object can not be initialized. However can be referenced.
            // AbstractClass abstractClass = new AbstractClass();
            
            AbstractClass.StaticMethod();
            
            // We can access the AbstractClass' properties by using its derived classes.
            AbstractClass abstractObject = new DerivedClass1();
            abstractObject.NormalMethod();
            
            abstractObject.AbstractMethod();
            abstractObject.abstractInt = 3;
        }

        public class DerivedClass1 : AbstractClass
        {
            public override void AbstractMethod()
            {
                Console.WriteLine("I am an override method from derived class 1.");
            }

            public override int abstractInt { get; set; }
        }

        public class DerivedClass2 : AbstractClass
        {
            public override void AbstractMethod()
            {
                Console.WriteLine("I am an override method from derived class 2.");
            }

            public override int abstractInt { get; set; }
        }

        // Can not mark as a sealed class (already you need to use inheritance from other classes)
        public abstract class AbstractClass
        {
            // Abstract class' static methods can called without creating objects of it.
            public static void StaticMethod()
            {
                Console.WriteLine("I am static method from abstract class.");
            }
            
            // Normal method
            public void NormalMethod()
            {
                Console.WriteLine("I am a normal method from abstract class.");
            }
            
            // Abstract methods can be either public or protected and cant be static.
            // Abstract methods must be overrided by derived classes.
            public abstract void AbstractMethod();
            
            // Properties can be set as an abstract too. Need to have get or set otherwise can not used as following.
            public abstract int abstractInt { get; set; } 

        }
    }
}
