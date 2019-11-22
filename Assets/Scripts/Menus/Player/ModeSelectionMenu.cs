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

	bool isEnabled;
	int selectedMode;

	GameManager gameManager;

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

		if (!gameManager.IsPaused( ) && !gameManager.IsOutOfMoves( ) && Utils.GetButtonValue("Y", player.GetID( )) && !player.IsDead( ) && !player.IsAtEnd( )) {
			ToggleModeSelect( );
		}
	}

	void ToggleModeSelect ( ) {
		if (isEnabled) {
			if (selectedMode != player.GetMode( )) {
				gameManager.DecrementMoves( );
			}

			player.SetMode(selectedMode);
		} else {
			boostRenderer.sprite = boost[Utils.BoolToInt(player.CanChangeBoost( ))];
			shrinkRenderer.sprite = shrink[Utils.BoolToInt(player.CanChangeShrink( ))];
			swapRenderer.sprite = swap[Utils.BoolToInt(player.CanChangeSwap( ))];
		}

		SetEnabled(!isEnabled);
	}

	public void SetEnabled (bool isEnabled) {
		this.isEnabled = isEnabled;

		player.GetComponent<SpriteRenderer>( ).sortingLayerName = isEnabled ? "Mini-UI" : "Player";

		SetTagEnabled(!isEnabled);

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(isEnabled);
		}
	}

	public void SetTagEnabled (bool isEnabled) {
		tagRenderer.enabled = isEnabled;
	}

	void UpdateModeSelection ( ) {
		float horiAxis = Utils.GetAxisRawValue("Horizontal", player.GetID( ));
		float vertAxis = Utils.GetAxisRawValue("Vertical", player.GetID( ));

		if (horiAxis > Constants.DEADZONE && player.CanChangeBoost( )) { // Right
			selectedMode = Constants.PLAYER_BOOST_MODE;
		} else if (horiAxis < -Constants.DEADZONE && player.CanChangeSwap( )) { // Left
			selectedMode = Constants.PLAYER_SWAP_MODE;
		} else if (vertAxis > Constants.DEADZONE) { // Up
			selectedMode = Constants.PLAYER_NORM_MODE;
		} else if (vertAxis < -Constants.DEADZONE && player.CanChangeShrink( )) { // Down
			selectedMode = Constants.PLAYER_SHRINK_MODE;
		}

		arrowAnim.SetInteger("selectedMode", selectedMode);
	}

	public bool IsEnabled ( ) {
		return isEnabled;
	}
}
