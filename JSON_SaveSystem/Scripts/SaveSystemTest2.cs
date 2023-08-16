using System;
using UnityEngine;

namespace JSON_SaveSystem.Scripts
{
    /// <summary>
    /// https://www.youtube.com/watch?v=6uMFEM-napE&ab_channel=CodeMonkey
    /// </summary>
    public class SaveSystemTest2 : MonoBehaviour
    {
        [SerializeField] private int _moneyamount;
        [SerializeField] private Vector3 _position;

        private string _mySaveObjectString;
        private void Awake()
        {
            SaveSystem.Init();
        }
		
        private void Start()
        {
            GenerateSaveObject();	
        }
	
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveSystem.Save(_mySaveObjectString);
            }
			
            if (Input.GetKeyDown(KeyCode.L))
            {
                var savedJson = SaveSystem.Load();
                SaveObject savedObject = SaveSystem.ConvertJsonToObject<SaveObject>(savedJson);
                PrintValues(savedObject);
            }
        }

        private void GenerateSaveObject()
        {
            SaveObject saveObject = new SaveObject()
            {
                Money = _moneyamount,
                Position = _position
            };

            string json = SaveSystem.ConvertObjectToJson(saveObject);
            _mySaveObjectString = json;
        }

        private void PrintValues(SaveObject saveObject)
        {
            Debug.Log(saveObject.Money +" " + saveObject.Position);
        }

		
    }
}