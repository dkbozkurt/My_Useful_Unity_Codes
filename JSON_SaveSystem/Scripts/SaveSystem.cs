using System.IO;
using UnityEngine;

namespace JSON_SaveSystem.Scripts
{
	/// <summary>
	/// Ref : https://www.youtube.com/watch?v=6uMFEM-napE&ab_channel=CodeMonkey
	/// </summary>
	
	public static class SaveSystem
	{
		private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

		public static void Init()
		{
			CheckSavingDirectory();
		}

		public static void Save(object saveObject, string targetFileName = "save")
		{
			string myJson = ConvertObjectToJson(saveObject);
			Save(myJson,targetFileName);
		}
		
		public static void Save(string dataToSave,string targetFileName = "save")
		{
			File.WriteAllText(SAVE_FOLDER+ targetFileName + ".txt", dataToSave);
		}

		public static T Load<T>(string targetFileName = "save")
		{
			string saveString = Load(targetFileName);
			T mySavedObject = ConvertJsonToObject<T>(saveString);
			return mySavedObject;
		}
		
		public static string Load(string targetFileName = "save")
		{
			if (!File.Exists(SAVE_FOLDER + targetFileName + ".txt")) 
				return null;
			
			string saveString = File.ReadAllText(SAVE_FOLDER + targetFileName + ".txt");
			return saveString;
		}
		
		public static string ConvertObjectToJson(object saveObject)
		{
			string jsonRepresentation = JsonUtility.ToJson(saveObject);
			return jsonRepresentation;
		}

		public static T ConvertJsonToObject<T>(string jsonRepresentation)
		{
			T savedObject = JsonUtility.FromJson<T>(jsonRepresentation);
			return savedObject;
		}
			
		private static void CheckSavingDirectory()
		{
			if(!Directory.Exists(SAVE_FOLDER))
				CreateSaveFolder();
		}

		private static void CreateSaveFolder()
		{
			Directory.CreateDirectory(SAVE_FOLDER);
		}
	}
}
