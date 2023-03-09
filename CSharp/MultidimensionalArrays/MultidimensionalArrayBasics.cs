// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp.MultidimensionalArrays
{
    /// <summary>
    /// Ref : https://www.w3schools.com/cs/cs_arrays_multi.php
    /// </summary>
    public class MultidimensionalArrayBasics
    {
        private int[,] _numbers = new int[,]{{1,4,2},{3,6,8}};
        
        private void PrintArrayElement()
        {
            Console.WriteLine(_numbers[0,2]); // 2
        }

        private void ChangeElement()
        {
            _numbers[1, 1] = 5;
            Console.WriteLine(_numbers[1, 1]); // 1 4 2 , 3 5 8
        }

    }
}