using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlescreenManager : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] Transitioner transitioner = null;
	[Header("UI")]
	[SerializeField] GameObject titlescreen = null;
	[SerializeField] GameObject levelSelect = null;
	[SerializeField] GameObject options = null;
	[SerializeField] GameObject credits = null;
	[SerializeField] Transform levels = null;
	[Header("First Buttons")]
	[SerializeField] GameObject titlescreenFirstButton = null;
	[SerializeField] GameObject levelSelectFirstButton = null;
	[SerializeField] GameObject optionsFirstButton = null;
	[SerializeField] GameObject creditsFirstButton = null;

	#region Unity Methods

	void Start ( ) {
		UpdateCompletedLevels( );
		UpdateMenuState( );
	}

	void Update ( ) {
		if (Utils.GetButtonValue("Start", Constants.PLAYER_1_ID)) {
			UpdateInputs( );
		}
	}

	#endregion

	#region Methods

	public void GoToState (int menuState) {
		Utils.TITLESCREEN_STATE = menuState;

		transitioner.RestartScene( );
	}

	public void UpdateMenuState ( ) {
		titlescreen.SetActive(Utils.TITLESCREEN_STATE == Constants.MENU_TITLESCREEN);
		levelSelect.SetActive(Utils.TITLESCREEN_STATE == Constants.MENU_LEVELSELECT);
		options.SetActive(Utils.TITLESCREEN_STATE == Constants.MENU_OPTIONS);
		credits.SetActive(Utils.TITLESCREEN_STATE == Constants.MENU_CREDTIS);

		UpdateInputs( );
	}

	public void UpdateCompletedLevels ( ) {
		try {
			bool[ ] completedLevels = SaveManager.LoadGame( ).completedLevels;

			for (int i = 0; i < completedLevels.Length; i++) {
				levels.GetChild(i).GetComponent<Image>( ).color = completedLevels[i] ? Color.yellow : Color.white;
			}
		} catch (Exception) {
			bool[ ] completedLevels = new bool[Constants.NUM_LEVELS];
			SaveManager.SaveGame(completedLevels);
		}
	}

	void UpdateInputs ( ) {
		if (Utils.TITLESCREEN_STATE == Constants.MENU_TITLESCREEN) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, titlescreenFirstButton);
		} else if (Utils.TITLESCREEN_STATE == Constants.MENU_LEVELSELECT) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, levelSelectFirstButton);
		} else if (Utils.TITLESCREEN_STATE == Constants.MENU_OPTIONS) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, optionsFirstButton);
		} else if (Utils.TITLESCREEN_STATE == Constants.MENU_CREDTIS) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, creditsFirstButton);
		}
	}

	public void Quit ( ) {
		Application.Quit( );
	}

	public void OpenURL (string url) {
		Application.OpenURL(url);
	}

	#endregion

	#region Getters



	#endregion

	#region Setters



	#endregion
}
