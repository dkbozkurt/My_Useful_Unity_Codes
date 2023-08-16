using System;
using UnityEngine;

namespace JSON_SaveSystem.Scripts
{
	/// <summary>
	/// https://www.youtube.com/watch?v=6uMFEM-napE&ab_channel=CodeMonkey
	/// </summary>
	public class SaveSystemTest1 : MonoBehaviour
	{
		[SerializeField] private int _moneyamount;
		[SerializeField] private Vector3 _position;

		private SaveObject _mySaveObject;
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
				SaveSystem.Save(_mySaveObject);
			}
			
			if (Input.GetKeyDown(KeyCode.L))
			{
				var loadedSaveObject = SaveSystem.Load<SaveObject>();
				PrintValues(loadedSaveObject);
			}
		}

		private void GenerateSaveObject()
		{
			SaveObject saveObject = new SaveObject()
			{
				Money = _moneyamount,
				Position = _position
			};

			_mySaveObject = saveObject;
		}

		private void PrintValues(SaveObject saveObject)
		{
			Debug.Log(saveObject.Money +" " + saveObject.Position);
		}

		
	}
}
