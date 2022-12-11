// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Custom_Attributes
{
    /// <summary>
    /// Custom Attributes
    /// Attributes are metadata extensions that give additional information to the compiler about the elements in
    /// the program code at runtime.To create custom attributes we must construct classes that derive from the
    /// System.Attribute class.
    ///
    /// Steps to create a Custom Attribute:
    ///
    /// AttributeUsageAttribute has three primary members as follows:
    ///
    /// 1. AttributeTargets.All specifies that the attribute may be applied to all parts of the program whereas
    /// Attribute.Class indicates that it may be applied to a class and AttributeTargets.Method to a method.
    ///
    /// [AttributeUsageAttribute( AttributeTargets.All )]
    ///
    /// 2. Inherited member is indicative of if the attribute might be inherited or not. It takes a boolean value
    /// (true/false). If this is not specified then the default is assumed to be true.
    /// 
    /// [AttributeUsage(AttributeTargets.All, Inherited = false)]
    ///
    /// 3.AllowMultiple member tells us if there can exist more than one instances of the attribute. It takes a
    /// boolean value as well. It is false by default.
    /// 
    /// [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    ///
    /// Ref : https://www.geeksforgeeks.org/custom-attributes-in-c-sharp/
    /// </summary>
    public class CustomAttributesREADME : MonoBehaviour
    { }
}
