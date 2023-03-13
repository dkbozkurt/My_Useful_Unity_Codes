// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp8
{
    /// <summary>
    /// Ref : https://www.geeksforgeeks.org/null-coalescing-operator-in-c-sharp/
    /// Ref : https://www.geeksforgeeks.org/null-coalescing-assignment-operator-in-c-sharp-8-0/
    /// </summary>
    public class NullCoalescing
    {
        // p ?? q Operator : It will return the value of its left-hand operand if it is not null.
        // If it is null, then it will evaluate the right-hand operand and returns its result.
        // q can not be null !!!
        public static void NullCoalescingOperator()
        {
            string? item_1 = null;
            string item_2 = "Dogukan Kaan";
            string item_3 = "Bozkurt";

            string item_4 = item_1 ?? item_2;
            item_3 = item_4 ?? item_2;
            
            Console.WriteLine("Value of item_4 is: {0} \n" +
                              "Value of item_3 is: {1}",item_4,item_3);

            int? tempInt_1 = null;
            int? tempInt_2 = null;
            int? tempInt_3 = 500;

            // Using Nested ?? operator
            int? tempInt_4 = tempInt_1 ?? tempInt_2 ?? tempInt_3;
            
            Console.WriteLine("Value of TempItem_4 is :{0}",tempInt_4);
        }
        
        // p ??= q Operator : It is known as a Null-coalescing assignment operator.
        // This operator is used to assign the value of its right-hand operand to its left-hand operand,
        // only if the value of the left-hand operand is null. If the left-hand operand evaluates to non-null,
        // then this operator does not evaluate its right-hand operand.
        public static void NullCoalescingAssignmentOperator()
        {
            string? item_1 = null;
            string? item_2 = null;
            string item_3 = "Dogukan Kaan";
            string item_4 = "Bozkurt";

            item_1 = item_1 ??= item_3;
            string item_5 = item_2 ??= item_4;
            
            Console.WriteLine("Value of item_1 is: {0}\n"+
                              "Value of item_6 is:{1}", item_1, item_5);

            int? element = null;

            element ??= 400;
            Console.WriteLine("Element is: {0}",element);

            int? tempInt_1 = null;
            int? tempInt_2 = null;
            int? tempInt_3 = 45;
            
            //Using Nested ??= operator
            int? result = tempInt_1 ??= tempInt_2 ??= tempInt_3;
            Console.WriteLine("Element is: {0}",result);
        }
    }
}