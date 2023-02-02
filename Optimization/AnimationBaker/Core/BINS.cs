///Modified from Neodrop's(neodrop@unity3d.ru) BinarySaver.
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BINS
{
    public static void Save(object obj,string folder, string fileName)
    {
		Debug.Log ("Save:"+folder+"/"+fileName);
		if(!System.IO.Directory.Exists(folder))
			System.IO.Directory.CreateDirectory(folder);
        FileStream fs = new FileStream(folder+"/"+fileName, FileMode.Create);
 
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, obj);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }


	public static void Save(object obj,string filePath)
	{
		Debug.Log ("Save:"+filePath);
		FileStream fs = new FileStream(filePath, FileMode.Create);
		
		BinaryFormatter formatter = new BinaryFormatter();
		try
		{
			formatter.Serialize(fs, obj);
		}
		catch (SerializationException e)
		{
			Debug.Log("Failed to serialize. Reason: " + e.Message);
			throw;
		}
		finally
		{
			fs.Close();
		}
	}

	
	public static object Load(string folder,string fileName)
	{
		Debug.Log ("Load:"+folder+"/"+fileName);
		if(!System.IO.Directory.Exists(folder))
			return null;
        if (!File.Exists(folder+"/"+fileName)) return null;

        FileStream fs = new FileStream(folder+"/"+fileName, FileMode.Open);
        object obj = null;
        try
        {
                BinaryFormatter formatter = new BinaryFormatter();

                obj = (object)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
        return obj;
    }


	public static object Load(string filePath)
	{
		Debug.Log ("Load:"+filePath);
		if (!File.Exists(filePath)){
			Debug.Log ("Can't find file:"+filePath);
			return null;
		}
		
		FileStream fs = new FileStream(filePath, FileMode.Open);
		object obj = null;
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			
			obj = (object)formatter.Deserialize(fs);
		}
		catch (SerializationException e)
		{
			Debug.Log("Failed to deserialize. Reason: " + e.Message);
			throw;
		}
		finally
		{
			fs.Close();
		}
		return obj;
	}


	public static void CreateFolder(string path){
		if(!System.IO.Directory.Exists(path)){
			System.IO.Directory.CreateDirectory(path);
			Debug.Log ("Folder created:"+path);
		}
	}

	public static bool FileExist(string filePath){
		bool result = false;
		if(File.Exists(filePath))
			result = true;
		return result;
	}
}