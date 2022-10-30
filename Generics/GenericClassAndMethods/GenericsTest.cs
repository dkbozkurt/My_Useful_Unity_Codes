// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

namespace Generics.GenericClassAndMethods
{
    /// <summary>
    /// Use with GenericsDataStore.cs
    /// 
    /// Ref : https://www.tutorialsteacher.com/csharp/csharp-generics#:~:text=Generic%20means%20the%20general%20form,without%20the%20specific%20data%20type.
    /// </summary>
    public class GenericsTest
    {
        private GenericsDataStore<string, int> _store = new GenericsDataStore<string, int>();

        private GenericDataStoreSecond<string> _cities = new GenericDataStoreSecond<string>();
        
        private void Awake()
        {
            AssignValuesForDataOne();
            AssignValuesForDataTwo();
            PrintValuesFromGenericMethod();
        }

        private void AssignValuesForDataOne()
        {
            _store.Key = "DogukanKaanBozkurt";
            _store.Value = 24;
        }

        private void AssignValuesForDataTwo()
        {
            _cities.AddOrUpdate(0,"Istanbul");
            _cities.AddOrUpdate(1,"Ankara");
            _cities.AddOrUpdate(2,"Izmir");
        }

        private void PrintValuesFromGenericMethod()
        {
            NonGenericClassWithGenericMethod printer = new NonGenericClassWithGenericMethod();
            printer.Print<int>(100);
            printer.Print(100); // Type infer from the specified value
            printer.Print<string>("Hello");
            printer.Print("World");
            
        }
    }
}