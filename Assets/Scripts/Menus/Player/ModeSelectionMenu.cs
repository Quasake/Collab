using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelectionMenu : MonoBehaviour {
	[Header("Sprites")]
	[SerializeField] Sprite[ ] tags = null;
	[SerializeField] Sprite[ ] boost = null;
	[SerializeField] Sprite[ ] shrink = null;
	[SerializeField] Sprite[ ] swap = null;
	[Header("Environment")]
	[SerializeField] Player player = null;
	[Header("Children")]
	[SerializeField] SpriteRenderer boostRenderer = null;
	[SerializeField] SpriteRenderer shrinkRenderer = null;
	[SerializeField] SpriteRenderer swapRenderer = null;
	[SerializeField] SpriteRenderer tagRenderer = null;
	[SerializeField] Animator arrowAnim = null;

	GameManager gameManager;

	bool isEnabled;
	int selectedMode;

	#region Unity Methods

	void Awake ( ) {
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>( );
	}

	void Start ( ) {
		tagRenderer.sprite = tags[player.GetID( )];

		SetEnabled(false);
	}

	void Update ( ) {
		if (isEnabled) {
			UpdateModeSelection( );

			if (gameManager.IsOutOfMoves( )) {
				SetEnabled(false);
			}
		}

		/*
		if (gameManager.IsAble( ) && Utils.GetButtonValue("Y", player.GetID( )) && player.IsAble( )) {
			ToggleModeSelect( );
		}
		*/

		if (gameManager.IsAble( ) && Input.GetButtonDown("Key-Menu-" + (player.GetID() + 1)) && player.IsAble( )) {
			ToggleModeSelect( );
		}
	}

	#endregion

	#region Methods
	void UpdateModeSelection ( ) {
		// float horiAxis = Utils.GetAxisRawValue("Horizontal", player.GetID( ));
		// float vertAxis = Utils.GetAxisRawValue("Vertical", player.GetID( ));

		float horiAxis = Input.GetAxisRaw("Key-Horizontal-" + (player.GetID( ) + 1));
		float vertAxis = Input.GetAxisRaw("Key-Vertical-" + (player.GetID( ) + 1));

		if (horiAxis > 0 && player.CanChangeBoost( )) { // Right
			selectedMode = Constants.PLAYER_BOOST_MODE;
		} else if (horiAxis < 0 && player.CanChangeSwap( )) { // Left
			selectedMode = Constants.PLAYER_SWAP_MODE;
		} else if (vertAxis > 0) { // Up
			selectedMode = Constants.PLAYER_NORM_MODE;
		} else if (vertAxis < 0 && player.CanChangeShrink( )) { // Down
			selectedMode = Constants.PLAYER_SHRINK_MODE;
		}

		arrowAnim.SetInteger("selectedMode", selectedMode);
	}

	void ToggleModeSelect ( ) {
		if (isEnabled) {
			player.SetMode(selectedMode);
		} else {
			boostRenderer.sprite = boost[Utils.BoolToInt(player.CanChangeBoost( ))];
			shrinkRenderer.sprite = shrink[Utils.BoolToInt(player.CanChangeShrink( ))];
			swapRenderer.sprite = swap[Utils.BoolToInt(player.CanChangeSwap( ))];
		}

		SetEnabled(!isEnabled);
	}

	#endregion

	#region Setters

	public void SetEnabled (bool isEnabled) {
		this.isEnabled = isEnabled;

		player.SetSortingLayer(isEnabled ? "Mini-UI" : "Player");

		SetTagEnabled(!isEnabled);

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(isEnabled);
		}
	}

	public void SetTagEnabled (bool isEnabled) {
		tagRenderer.enabled = isEnabled;
	}

	#endregion

	#region Getters

	public bool IsEnabled ( ) {
		return isEnabled;
	}

	#endregion
}
