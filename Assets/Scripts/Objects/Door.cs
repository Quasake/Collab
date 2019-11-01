using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Component {
	public bool isActiveTop;

	// 0 = off, 1 = on, 2 = none
	public Sprite[ ] variations;

	public Transform door;
	public SpriteRenderer topSpriteRenderer;
	public SpriteRenderer bottomSpriteRenderer;

	protected override void UpdateSprites ( ) {
		topSpriteRenderer.sprite = variations[isActiveTop ? (isActive ? 1 : 0) : 2];
		bottomSpriteRenderer.sprite = variations[isActiveTop ? 2 : (isActive ? 1 : 0)];
	}
}
