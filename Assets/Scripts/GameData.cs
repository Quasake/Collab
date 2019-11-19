using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
	public bool[ ] completedLevels;

	public GameData (bool[ ] completedLevels) {
		this.completedLevels = completedLevels;
	}
}
