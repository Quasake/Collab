using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {
	static EventSystem eventSystem = null;
	static StandaloneInputModule inputModule = null;

	void Awake ( ) {
		eventSystem = GetComponent<EventSystem>( );
		inputModule = GetComponent<StandaloneInputModule>( );
	}

	public static void SetInputs (int playerID, GameObject firstButton) {
		playerID++;

		inputModule.horizontalAxis = "Horizontal-" + playerID;
		inputModule.verticalAxis = "Vertical-" + playerID;
		inputModule.submitButton = "A-" + playerID;
		inputModule.cancelButton = "B-" + playerID;

		eventSystem.SetSelectedGameObject(firstButton, new BaseEventData(eventSystem));
	}
}
