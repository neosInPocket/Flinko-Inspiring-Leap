using System.IO;
using UnityEngine;

public class KeyValueData : MonoBehaviour
{
	[SerializeField] private bool deleteStartData;
	[SerializeField] private Data defaultSave;
	private static string path => Application.persistentDataPath + "/MainProcess_SaveFile.json";
	public static Data SavedData;
	public static Data defaultSaveFile;

	private void Awake()
	{
		defaultSaveFile = (Data)defaultSave.Clone();

		if (!deleteStartData)
		{
			Load();
		}
		else
		{
			DefaultValues();
		}
	}

	public static void SaveProgress()
	{
		if (!File.Exists(path))
		{
			NewFile();
		}
		else
		{
			Writer();
		}
	}

	private void Load()
	{
		if (!File.Exists(path))
		{
			NewFile();
		}
		else
		{
			LoadFile();
		}
	}

	private void DefaultValues()
	{
		NewFile();
	}

	private static void NewFile()
	{
		SavedData = (Data)defaultSaveFile.Clone();
		File.WriteAllText(path, JsonUtility.ToJson(SavedData, prettyPrint: true));
	}

	private static void Writer()
	{
		File.WriteAllText(path, JsonUtility.ToJson(SavedData, prettyPrint: true));
	}

	private static void LoadFile()
	{
		string jsonFile = File.ReadAllText(path);
		SavedData = JsonUtility.FromJson<Data>(jsonFile);
	}
}
