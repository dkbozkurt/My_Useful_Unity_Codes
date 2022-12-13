// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Custom_Attributes.NameAndActionAttribute.Scripts
{
    /// <summary>
    ///  Ref : https://www.geeksforgeeks.org/custom-attributes-in-c-sharp/
    /// </summary>
    public class Employer
    {
        private int _id;
        private string _name;

        public Employer(int i, string n)
        {
            _id = i;
            _name = n;
        }
  
        [NameAndActionAttribute("Accessor", "Gives value of Employer Id")] 
        public int GetId()
        {
            return _id;
        }
  
        [NameAndActionAttribute("Accessor", "Gives value of Employer Name")] 
        public string GetName()
        {
            return _name;
        }
    }
}
