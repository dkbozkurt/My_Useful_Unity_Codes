// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace ShowAndDrawIfAttribute
{
    public enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        And,
        // A field is visible/enable if at lease ONE condition is true.
        Or
    }

    public enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DontDraw,   
        // If condition(s) are false, just set the field as disabled.
        JustDisable
    }
    
    /// <summary>
    /// 
    /// Ref : https://stackoverflow.com/questions/58441744/how-to-enable-disable-a-list-in-unity-inspector-using-a-bool
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false,Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public ActionOnConditionFail Action { get; private set; }
        public ConditionOperator Operator { get; private set; }
        public string[] Conditions { get; private set; }

        public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator,
            params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }
    }
}
