// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp8
{
    /// <summary>
    /// Ref : https://www.geeksforgeeks.org/range-and-indices-in-c-sharp-8-0/
    /// </summary>
    public class RangeAndIndices
    {
        // ^Operator : Returns an index that is relative to the end of the sequence or collection
        public static void IndexFromTheEndOperator()
        {
            int[] myArray = new int[] {34, 56, 77, 88, 90, 45};
  
            Console.WriteLine("Values of the specified indexes:");
            Console.WriteLine(myArray[0]);
            Console.WriteLine(myArray[1]);
            Console.WriteLine(myArray[2]);
            Console.WriteLine(myArray[3]);
            Console.WriteLine(myArray[4]);
            Console.WriteLine(myArray[5]);
            
            // Console.WriteLine("The end values of the specified indexes:");
            Console.WriteLine(myArray[^1]);
            Console.WriteLine(myArray[^2]);
            Console.WriteLine(myArray[^3]);
            Console.WriteLine(myArray[^4]);
            Console.WriteLine(myArray[^5]);
            Console.WriteLine(myArray[^6]);

            // Declare and index as a variable
            Index i = ^6;
            var value = myArray[i];
            
            Console.WriteLine("Number: "+ value);
        }
        
        // .. Operator: It is known as the range operator. And it specifies the start and end as its operands of the given range
        public static void RangeOperator()
        {
            string[] marketingTeam = new string[] { "burak","dogukan","eymen","tuana","onur","mert"};
            
            Console.WriteLine("Name of the artists: ");
            var P_A = marketingTeam[2..6];
            foreach (var employee in P_A)
            {
                Console.WriteLine($"[{employee}]");
            }
            
            Console.WriteLine("Name of the developers: ");
            var P_D = marketingTeam[..1];
            foreach (var employee in P_D)
            {
                Console.WriteLine($"[{employee}]");
            }
            
            // Declaring a range as a variable
            Range num = 1..3;
            string[] val = marketingTeam[num];

            foreach (var child in val)
            {
                Console.WriteLine($"[{child}]");
            }
        }
        
        
    }
}