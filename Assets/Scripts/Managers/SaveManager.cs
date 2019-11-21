using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {
	public static void UpdateLevel (int index) {
		bool[ ] completedLevels;

		try {
			completedLevels = LoadGame( ).completedLevels;
		} catch (Exception) {
			completedLevels = new bool[Constants.NUM_LEVELS];
		}

		completedLevels[index] = true;

		for (int i = 0; i < completedLevels.Length; i++) {
			Debug.Log(completedLevels[i]);
		}

		SaveGame(completedLevels);
	}

	public static void SaveGame (bool[ ] completedLevels) {
		string path = Application.persistentDataPath + "/gamedata.collab";

		BinaryFormatter formatter = new BinaryFormatter( );
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
			Debug.Log("Save file not found at: " + path);

			return null;
		}
	}

}
