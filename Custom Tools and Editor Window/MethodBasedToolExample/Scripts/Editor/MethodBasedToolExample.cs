using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.MethodBasedToolExample.Scripts.Editor
{
    public class MethodBasedToolExample : MonoBehaviour
    {
        private static GameObject _testGameObject;
    
        [MenuItem("Tools/MethodBasedTool/Print Hi")]
        public static void PrintHi()
        {
            Debug.Log("Hi !");
        }

        [MenuItem("Tools/MethodBasedTool/GenerateEmptyGameObjectWithAudioSource")]
        public static void CreateGameObject()
        {
            if(GameObject.Find("MyTestObject") == null)
            {
                _testGameObject= new GameObject("MyTestObject");
                _testGameObject.transform.position = Vector3.zero;
                Debug.Log("MyTestObject successfully Instantiated ! ");
            }
            else
            {
                Debug.Log("MyTestObject already added into the scene !!!");
            }

            _testGameObject.AddComponent<AudioSource>();

        }
    }
}
