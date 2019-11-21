using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu {
	bool isPaused = false;
	int pausedPlayerID = -1;

	void Start ( ) {
		SetEnabled(false);
	}

	public void Pause (int playerID) {
		pausedPlayerID = playerID;
		isPaused = true;

		SetInputs(playerID, firstButton);
		SetEnabled(true);
	}

	public void UnPause ( ) {
		pausedPlayerID = -1;
		isPaused = false;

		SetEnabled(false);
	}

	public bool IsPaused ( ) {
		return isPaused;
	}
}
