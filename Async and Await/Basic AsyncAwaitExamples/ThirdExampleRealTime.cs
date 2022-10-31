// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.Basic_AsyncAwaitExamples
{
    /// <summary>
    ///
    /// Reading all the characters from a large text file asynchronously and get the total length of all the characters.
    ///
    /// We are using async programming to read all the contents from the file, so it will not wait to get a return value
    /// from this method and execute the other lines of code. Still it has to wait for the line of code given below because
    /// we are using await keywords.
    /// 
    /// Ref : https://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/
    /// </summary>

    public class ThirdExampleRealTime : MonoBehaviour
    {
        private void Awake()
        {
            Main();
        }

        private void Main()
        {
            Task task = new Task(CallMethod);
            task.Start();
            task.Wait();
        }

        private async void CallMethod()
        {
            string filePath = "D:\\sampleFile.txt";
            Task<int> task = ReadFile(filePath);
            
            Debug.Log("Other Work 1");
            Debug.Log("Other Work 2");
            Debug.Log("Other Work 3");

            int length = await task;
            Debug.Log("Total length: " + length);
            
            Debug.Log("After work 1");
            Debug.Log("After work 2");
        }

        private async Task<int> ReadFile(string file)
        {
            int length = 0;
            
            Debug.Log("File reading is stating");
            using (StreamReader reader = new StreamReader(file))
            {
                // Reads all characters from the current position to the end of the stream asynchronously
                // and returns them as one string.

                string s = await reader.ReadToEndAsync();

                length = s.Length;
            }
            
            Debug.Log("File reading is completed");
            return length;
        }
    }
}
