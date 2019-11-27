using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager {
	public static void UpdateLevel (int index) {
		bool[ ] completedLevels = LoadLevels( );

		completedLevels[index] = true;

		SaveLevels(completedLevels);
	}

	public static void SaveLevels (bool[ ] completedLevels) {
		BinaryFormatter formatter = new BinaryFormatter( );
		FileStream stream = new FileStream(Constants.PATH_LEVELS, FileMode.Create);

		formatter.Serialize(stream, completedLevels);
		stream.Close( );
	}

	public static bool[ ] LoadLevels ( ) {
		if (File.Exists(Constants.PATH_LEVELS)) {
			BinaryFormatter formatter = new BinaryFormatter( );
			FileStream stream = new FileStream(Constants.PATH_LEVELS, FileMode.Open);

			bool[ ] completedLevels = formatter.Deserialize(stream) as bool[ ];

			stream.Close( );

			return completedLevels;
		}

		return new bool[Constants.NUM_LEVELS];
	}
}
