using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
	[Header("Sprites")]
	[SerializeField] Sprite[ ] variations = null;

	SpriteRenderer spriteRenderer;

	#region Unity Methods

	void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	void Start ( ) {
		spriteRenderer.sprite = variations[Utils.GetRandomInteger(0, variations.Length - 1)];
	}

	#endregion
}
