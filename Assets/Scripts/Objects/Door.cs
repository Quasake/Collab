using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : WireComponent {
	[Header("Sprites")]
	[SerializeField] Sprite[ ] variations = null; // 0 = off, 1 = on
	[Header("Children")]
	[SerializeField] Transform door = null;

	private void Start ( ) {
		SetActive(isActive);
	}

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[(isActive ? 1 : 0)];
	}

	private void Update ( ) {
		Vector3 doorMove = (isActive ? -1 : 1) * new Vector3(0, Constants.DOOR_MOVESPEED);
		door.position = new Vector3(door.position.x, Utils.Limit(door.position.y + doorMove.y, -1.5f, 1.5f));
	}
}
