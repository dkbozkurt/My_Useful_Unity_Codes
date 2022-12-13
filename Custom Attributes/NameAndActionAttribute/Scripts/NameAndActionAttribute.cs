// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Reflection;
using UnityEngine;

namespace Custom_Attributes.NameAndActionAttribute.Scripts
{
    /// <summary>
    /// Ref : https://www.geeksforgeeks.org/custom-attributes-in-c-sharp/
    /// </summary>
    [AttributeUsage(AttributeTargets.All)] 
    public class NameAndActionAttribute : Attribute
    {
        private string _title;
        private string _description;

        public NameAndActionAttribute(string title, string description)
        {
            _title = title;
            _description = description;
        }

        public static void AttributeDisplay(Type classType)
        {
            Debug.Log($"Method of class {classType.Name}");

            MethodInfo[] methods = classType.GetMethods();

            for (int i = 0; i < methods.GetLength(0); i++)
            {
                object[] attributesArray = methods[i].GetCustomAttributes(true);

                foreach (Attribute item in attributesArray)
                {
                    if (item is NameAndActionAttribute)
                    {
                        NameAndActionAttribute attributeObject = (NameAndActionAttribute) item;
                        Debug.Log($"{methods[i].Name} - {attributeObject._title}, {attributeObject._description}");
                    }
                }
            }
        }
    }
}
