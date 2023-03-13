// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;

namespace CSharp8
{
    /// <summary>
    /// Running c# in rider : https://www.youtube.com/watch?v=NXxUEfLXqKc&t=375s&ab_channel=HindiLife
    /// 
    /// Ref :  https://learn.microsoft.com/tr-tr/dotnet/csharp/language-reference/proposals/csharp-8.0/nullable-reference-types
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            #region Readonly Members
            
            // ReadonlyMembers.StartReadonlyClass();
            // ReadonlyMembers.StartReadonlyStruct();
            
            #endregion

            #region Interpolated Verbatim Strings

            // InterpolatedVerbatimStrings.Start();

            #endregion

            #region Pattern Matching Enhancements

            // PatternMatchingEnhancements.SwitchExpressions();
            // PatternMatchingEnhancements.SwitchExpression_WithEnums();
            // PatternMatchingEnhancements.PropertyPattern_CalculateSalesTax();
            // PatternMatchingEnhancements.TuplePattern_OrderDiscount();
            // PatternMatchingEnhancements.PositionalPattern_Shipping();

            #endregion

            #region Range and Indices

            //RangeAndIndices.IndexFromTheEndOperator();
            //RangeAndIndices.RangeOperator();
            
            #endregion

            #region Null-Coalescing

            // NullCoalescing.NullCoalescingOperator();
            // NullCoalescing.NullCoalescingAssignmentOperator();

            #endregion
        }
    }
}