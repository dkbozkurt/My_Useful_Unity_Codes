using System;
using SerializableDictionary.Runtime;

namespace SerializableDictionary.Test
{
    [Serializable]
    public class IntStringDictionary : SerializableDictionary<int,string> { }
}
