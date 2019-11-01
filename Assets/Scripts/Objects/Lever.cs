using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Component {
	[Header("Sprites")]
	[SerializeField] private Sprite[ ] variations;

	private SpriteRenderer spriteRenderer;

	private void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[isActive ? 1 : 0];
	}

	public void Toggle ( ) {
		SetIsActive(!isActive);
	}
}
