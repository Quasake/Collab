using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlescreenManager : MonoBehaviour {
	public static int TITLESCREEN_STATE = Constants.MENU_TITLESCREEN;

	[Header("Environment")]
	[SerializeField] TransitionManager transitionManager = null;
	[Header("UI")]
	[SerializeField] TitleMenu titleMenu = null;
	[SerializeField] LevelMenu levelMenu = null;
	[SerializeField] OptionsMenu optionsMenu = null;
	[SerializeField] CreditsMenu creditsMenu = null;

	Menu enabledMenu;

	#region Unity Methods

	void Start ( ) {
		UpdateMenu( );
	}

	void Update ( ) {
		if (Utils.GetButtonValue("Start", Constants.PLAYER_1_ID)) {
			UpdateMenu( );
		}
	}

	#endregion

	#region Methods

	public void GoToState (int menuState) {
		TITLESCREEN_STATE = menuState;

		transitionManager.RestartScene( );
	}

	public void UpdateMenu ( ) {
		titleMenu.SetEnabled(TITLESCREEN_STATE == Constants.MENU_TITLESCREEN);
		levelMenu.SetEnabled(TITLESCREEN_STATE == Constants.MENU_LEVELSELECT);
		optionsMenu.SetEnabled(TITLESCREEN_STATE == Constants.MENU_OPTIONS);
		creditsMenu.SetEnabled(TITLESCREEN_STATE == Constants.MENU_CREDTIS);

		enabledMenu = titleMenu.IsEnabled( ) ? titleMenu : (levelMenu.IsEnabled( ) ? levelMenu : (optionsMenu.IsEnabled( ) ? optionsMenu : (creditsMenu.IsEnabled( ) ? (Menu) creditsMenu : null)));

		Menu.SetInputs(Constants.PLAYER_1_ID, enabledMenu.GetFirstButton( ));
	}

	#endregion

	#region Getters



	#endregion

	#region Setters



	#endregion
}
