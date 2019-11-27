using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : WireComponent {
	[Header("Sprites")]
	[SerializeField] private Sprite[ ] variations = null;

	#region Unity Methods

	private void Start ( ) {
		SetActive(isActive);
	}

	#endregion

	#region Methods

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[isActive ? 1 : 0];
	}

	public void Toggle ( ) {
		SetActive(!isActive);
	}

	#endregion
}
