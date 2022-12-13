using UnityEngine;

namespace Custom_Attributes.NameAndActionAttribute.Scripts
{
    public class Employee : MonoBehaviour
    {
        private int _id;
        private string _name;
  
        public Employee(int id, string name)
        {
            _id = id;
            _name = name;
        }
        
        [NameAndActionAttribute("Accessor", "Gives value of Employee Id")] 
        public int GetId()
        {
            return _id;
        }
        
        [NameAndActionAttribute("Accessor", "Gives value of Employee Name")] 
        public string GetName()
        {
            return _name;
        }
    }
}
