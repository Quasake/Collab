using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour {
	[Header("Superclass [Menu]")]
	[SerializeField] protected GameObject firstButton = null;

	Image image;

	protected bool isEnabled = true;

	#region Unity Methods

	void Awake ( ) {
		image = GetComponent<Image>( );
	}

	#endregion

	#region Methods

	#endregion

	#region Setters

	public static void SetInputs (int playerID, GameObject firstButton) {
		playerID++;

		EventSystem eventSystem = EventSystem.current;
		StandaloneInputModule inputModule = eventSystem.GetComponent<StandaloneInputModule>( );

		// inputModule.horizontalAxis = "Horizontal-" + playerID;
		// inputModule.verticalAxis = "Vertical-" + playerID;
		// inputModule.submitButton = "A-" + playerID;
		// inputModule.cancelButton = "B-" + playerID;
		// eventSystem.SetSelectedGameObject(firstButton, new BaseEventData(eventSystem));
	}

	public void SetEnabled (bool isEnabled) {
		this.isEnabled = isEnabled;
		if (image != null) {
			image.enabled = isEnabled;
		}

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(isEnabled);
		}
	}

	#endregion

	#region Getters

	public bool IsEnabled ( ) {
		return isEnabled;
	}

	public GameObject GetFirstButton ( ) {
		return firstButton;
	}

	#endregion
}
