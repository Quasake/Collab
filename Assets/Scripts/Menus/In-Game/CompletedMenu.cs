using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedMenu : Menu {
	bool isCompleted = false;

	void Start ( ) {
		SetEnabled(false);
	}

	void Update ( ) {
		if (GameManager.GetPlayer1( ).IsAtEnd( ) && GameManager.GetPlayer2( ).IsAtEnd( )) {
			if (!isCompleted) {
				isCompleted = true;

				SetEnabled(true);
				SetInputs(Constants.PLAYER_1_ID, firstButton);

				SaveManager.UpdateLevel(SceneManager.GetActiveScene( ).buildIndex - 1);
			}
		}
	}
}
