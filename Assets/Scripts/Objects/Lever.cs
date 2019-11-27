using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : WireComponent {
	[Header("Sprites")]
	[SerializeField] Sprite[ ] variations = null;
	[Header("Sounds")]
	[SerializeField] AudioClip toggleLever = null;

	#region Unity Methods

	void Start ( ) {
		SetActive(isActive);
	}

	#endregion

	#region Methods

	protected override void UpdateSprites ( ) {
		spriteRenderer.sprite = variations[isActive ? 1 : 0];
	}

	public void Toggle ( ) {
		Utils.PlaySound(audioSource, toggleLever);

		SetActive(!isActive);
	}

	#endregion
}
