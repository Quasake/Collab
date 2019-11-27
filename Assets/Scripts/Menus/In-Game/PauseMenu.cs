using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu {
	bool isPaused = false;

	#region Unity Methods 

	void Start ( ) {
		SetEnabled(false);
	}

	#endregion

	#region Methods

	public void Pause (int playerID) {
		isPaused = true;

		SetInputs(playerID, firstButton);
		SetEnabled(true);
	}

	public void UnPause ( ) {
		isPaused = false;

		SetEnabled(false);
	}

	#endregion

	#region Setters

	#endregion

	#region Getters

	public bool IsPaused ( ) {
		return isPaused;
	}

	#endregion
}
