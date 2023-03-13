using System;

namespace CSharp8
{
    /// <summary>
    /// When we use a @ token before $ token, then the compiler gives an error.
    /// This problem is solved in C# 8.0, now you are allowed to use a @ token
    /// before $ token or $ token before @ token
    /// 
    /// Ref : https://www.geeksforgeeks.org/interpolated-verbatim-strings-in-c-sharp-8-0/
    /// </summary>
    public class InterpolatedVerbatimStrings
    {
        public static void Start()
        {
            int Base = 40;
            int Height = 80;
            int area = (Base * Height) / 2;
            
            // We used both $@ and @$ below.
            Console.WriteLine($@"Height = ""{Height}"" and Base = ""{Base}""");
            Console.WriteLine(@$"Area = ""{area}""");
        }
    }
}