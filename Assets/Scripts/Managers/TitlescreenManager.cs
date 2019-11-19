using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenManager : MonoBehaviour {
	[Header("UI")]
	[SerializeField] GameObject titlescreen = null;
	[SerializeField] GameObject levelSelect = null;
	[SerializeField] GameObject options = null;
	[SerializeField] GameObject credits = null;
	[Header("First Buttons")]
	[SerializeField] GameObject titlescreenFirstButton = null;
	[SerializeField] GameObject levelSelectFirstButton = null;
	[SerializeField] GameObject optionsFirstButton = null;
	[SerializeField] GameObject creditsFirstButton = null;
	[Header("Variables")]
	[SerializeField] int menuState = Constants.MENU_TITLESCREEN;

	void Start ( ) {
		SetMenuState(Constants.MENU_TITLESCREEN);
	}

	void Update ( ) {

	}

	public void SetMenuState (int menuState) {
		this.menuState = menuState;

		titlescreen.SetActive(menuState == Constants.MENU_TITLESCREEN);
		levelSelect.SetActive(menuState == Constants.MENU_LEVELSELECT);
		options.SetActive(menuState == Constants.MENU_OPTIONS);
		credits.SetActive(menuState == Constants.MENU_CREDTIS);

		if (menuState == Constants.MENU_TITLESCREEN) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, titlescreenFirstButton);
		} else if (menuState == Constants.MENU_LEVELSELECT) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, levelSelectFirstButton);
		} else if (menuState == Constants.MENU_OPTIONS) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, optionsFirstButton);
		} else if (menuState == Constants.MENU_CREDTIS) {
			MenuManager.SetInputs(Constants.PLAYER_1_ID, creditsFirstButton);
		}
	}

	public void Quit ( ) {
		Application.Quit( );
	}
}
