using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {

	public static void CompleteLevel (int index) {
		bool[ ] completedLevels = new bool[Constants.NUM_LEVELS];
		try {
			completedLevels = SaveManager.LoadGame( ).completedLevels;

			completedLevels[index] = true;
		} catch (Exception) {
		}

		SaveGame(completedLevels);
	}

	public static void SaveGame (bool[ ] completedLevels) {
		BinaryFormatter formatter = new BinaryFormatter( );
		string path = Application.persistentDataPath + "/gamedata.collab";
		FileStream stream = new FileStream(path, FileMode.Create);

		GameData data = new GameData(completedLevels);

		formatter.Serialize(stream, data);
		stream.Close( );
	}

	public static GameData LoadGame ( ) {
		string path = Application.persistentDataPath + "/gamedata.collab";
		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter( );
			FileStream stream = new FileStream(path, FileMode.Open);

			GameData data = formatter.Deserialize(stream) as GameData;
			stream.Close( );

			return data;
		} else {
			Debug.LogError("Save file not found at: " + path);
			return null;
		}
	}

}
