// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;

namespace Generics.GenericClassAndMethods
{
    /// <summary>
    /// Use with GenericsTest.cs
    ///
    /// You can create an instance of generic classes by specifying an actual type in angle brackets.
    /// The following creates an instance of the generic class GenericDataStore.
    ///
    /// Note : A generic class can include generic fields. However, IT CANNOT BE INITIALIZED !
    ///
    /// A method declared with the type parameters for its return type or parameters is called a generic method.
    /// 
    ///  A non-generic class can include generic methods by specifying a type parameter in angle brackets with the method name, as shown below.
    ///
    /// - - - ADVANTAGES OF GENERICS - - -
    ///
    /// 1) Generics increase the reusability of the code. You don't need to write code to handle different data types.
    /// 2) Generics are type-safe. You get compile-time errors if you try to use a different data type than the one
    /// specified in the definition.
    /// 3) Generic has a performance advantage because it removes the possibilities of boxing and unboxing.
    /// 
    /// Ref : https://www.tutorialsteacher.com/csharp/csharp-generics#:~:text=Generic%20means%20the%20general%20form,without%20the%20specific%20data%20type.
    /// </summary>

    // Define Generic Class
    public class GenericsDataStore<TKey,TValue>
    {
        public TKey Key { get; set; }
        
        public TValue Value { get; set; }
        
        // Instantiating Generic Class
        
    }

    public class GenericDataStoreSecond<T>
    {
        private static int _arraySize = 10;
        private T[] _data = new T[_arraySize];

        // Generics Method
        public void AddOrUpdate(int index, T item)
        {
            if (index >= 0 && index < _arraySize)
                _data[index] = item;
        }

        // Generics Method
        public T GetData(int index)
        {
            if (index >= 0 && index < _arraySize)
                return _data[index];
            else
                return default(T);
        }
        
        // Generic Method Overloading
        /// <summary>
        /// The generic parameter type can be used with multiple parameters with or without non-generic parameters
        /// and return type. The followings are valid generic method overloading.
        /// </summary>
        public void AddOrUpdate(T data1, T data2) { }
        public void AddOrUpdate<U>(T data1, U data2) { }
        public void AddOrUpdate(T data) { }
        
    }

    public class NonGenericClassWithGenericMethod
    {
        public void Print<T>(T data)
        {
             Console.WriteLine("Printed Data : "+data);
        }
    }
}
