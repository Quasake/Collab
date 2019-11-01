using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Component {
	[Header("Sprites")]
	[SerializeField] private Sprite[ ] variations;

	[Header("Environment")]
	[SerializeField] private GameManager gameManager;

	private SpriteRenderer spriteRenderer;

	private void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		transform.localScale += Constants.GAP_FIX_NUMBER * new Vector3(1, 1);
	}

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[isActive ? 1 : 0];

		gameManager.SetTileTexture(this, isActive);
	}

	public void Toggle ( ) {
		SetIsActive(!isActive);
	}
}
