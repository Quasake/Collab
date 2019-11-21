using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {
	public static void UpdateLevel (int index) {
		bool[ ] completedLevels = LoadGame( );

		completedLevels[index] = true;

		SaveGame(completedLevels);
	}

	public static void SaveGame (bool[ ] completedLevels) {
		string path = Application.persistentDataPath + "/gamedata.collab";

		BinaryFormatter formatter = new BinaryFormatter( );
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, completedLevels);
		stream.Close( );
	}

	public static bool[ ] LoadGame ( ) {
		string path = Application.persistentDataPath + "/gamedata.collab";

		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter( );
			FileStream stream = new FileStream(path, FileMode.Open);

			bool[ ] completedLevels = formatter.Deserialize(stream) as bool[ ];

			stream.Close( );

			return completedLevels;
		} else {
			Debug.Log("Save file not found at: " + path);

			return new bool[Constants.NUM_LEVELS];
		}
	}

}
