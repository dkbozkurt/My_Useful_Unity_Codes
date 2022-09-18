// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dict
{
    /// <summary>
    ///
    /// Dict
    ///
    /// 
    /// Ref : https://stackoverflow.com/questions/17047602/proper-way-to-initialize-a-c-sharp-dictionary-with-values#:~:text=With%20C%23%206.0%2C%20you%20can,%22three%22%5D%20%3D%203%20%7D%3B
    /// https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/#:~:text=In%20C%23%2C%20Dictionary%20is%20a,Dictionary%20is%20defined%20under%20System
    /// https://www.tutorialsteacher.com/csharp/csharp-dictionary#:~:text=Access%20Dictionary%20Elements,KeyValuePair%20from%20the%20specified%20index 
    /// </summary>

    public class DictionaryBasics
    {
        private Dictionary<string, int> _testDictionary;
        
        private void Start()
        {
            InitializeDictionary();
            AddingToDict(_testDictionary);
            PrintingDict(_testDictionary);
            RemoveElementAndClearDict(_testDictionary);
            CheckIfContainsKey(_testDictionary);
            CheckIfContainsValue(_testDictionary);
            AccessValueByKeyByUsingTry(_testDictionary);
            AccessDictionaryElementByIndex(_testDictionary);
        }

        private void InitializeDictionary()
        {
            // Version 1
            // _testDictionary = new Dictionary<string, int>
            // {
            //     {"Dogukan",1},
            //     {"Kaan",2},
            //     {"Bozkurt",3}
            // };
            
            // Version 2
            _testDictionary =  new Dictionary<string, int>
            {
                ["Dogukan"] =1,
                ["Kaan"] = 2,
                ["Bozkurt"] =3
            };
        }

        private void AddingToDict(Dictionary<string,int> dictionary)
        {
            dictionary.Add("Age",24);
            dictionary.Add("PhoneNumber",514);
        }
        
        private void PrintingDict(Dictionary<string,int> dictionary)
        {
            foreach (KeyValuePair<string,int> element in dictionary)
            {
                Console.WriteLine("{Key {0}; Value {1}",element.Key,element.Value);
            }
            Console.WriteLine();
        }

        private void RemoveElementAndClearDict(Dictionary<string, int> dictionary)
        {
            dictionary.Remove("Kaan");
            PrintingDict(dictionary);
            
            dictionary.Clear();
            PrintingDict(dictionary);
        }

        private void CheckIfContainsKey(Dictionary<string, int> dictionary)
        {
            if(dictionary.ContainsKey("Dogukan")) Debug.Log("Key is found");
            else Debug.Log("Key couldn't found");
            
        }

        private void CheckIfContainsValue(Dictionary<string, int> dictionary)
        {
            if(dictionary.ContainsValue(24)) Debug.Log("Value is found");
            else Debug.Log("Value couldn't found");
        }

        private void AccessValueByKeyByUsingTry(Dictionary<string,int> dictionary)
        {
            int result;
            if (dictionary.TryGetValue("Bozkurt", out result))
            {
                Console.WriteLine("The Value is : " + result);
            }
        }

        private void AccessDictionaryElementByIndex(Dictionary<string, int> dictionary)
        {
            for (int i = 0; i< dictionary.Count; i++)
            {
                Console.WriteLine("Key -> {0}, Value ->{1}",dictionary.ElementAt(i).Key,dictionary.ElementAt(i).Value);
            }
        }
        
        
    }
}
