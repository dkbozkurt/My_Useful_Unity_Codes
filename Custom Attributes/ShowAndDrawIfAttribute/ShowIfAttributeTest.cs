// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace ShowAndDrawIfAttribute
{
    /// <summary>
    /// Ref : https://stackoverflow.com/questions/58441744/how-to-enable-disable-a-list-in-unity-inspector-using-a-bool
    /// </summary>
    public class ShowIfAttributeTest : MonoBehaviour
    {
        /// <summary>
        /// Using a field to hide/show another field
        /// </summary>
        public bool showHideList = false;
        [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And,
            nameof(showHideList))]
        public string aField = "item 1";
        
        [Space]
        [Space]
        /// <summary>
        /// Using a field to enable/disable another field.
        /// </summary>
        public bool enableDisableList = false;
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And,
            nameof(enableDisableList))]
        public string anotherField = "item 2";
        
        [Space]
        [Space]
        /// <summary>
        /// Using a method to get a condition value.
        /// </summary>
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And,
            nameof(CalculateIsEnabled))]
        public string yetAnotherField = "one more";
        public bool CalculateIsEnabled()
        {
            // return true
            return false;
        }

        [Space]
        [Space]
        /// <summary>
        /// Using multiple conditions on the same field.
        /// </summary>
        public bool condition1;
        public bool condition2;
        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And,
            nameof(condition1), nameof(condition2))]
        public string oneLastField = "last field";
        

    }
}