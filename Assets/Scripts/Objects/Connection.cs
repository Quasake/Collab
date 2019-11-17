using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : WireComponent {
	[Header("Sprites")]
	[SerializeField] private Sprite[ ] variations = null; // 0 = off, 1 = on, 2 = none, 3 = special

	private void Start ( ) {
		SetActive(isActive);
	}

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[(isActive ? 1 : 0)];
	}
}
