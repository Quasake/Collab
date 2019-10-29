using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
	public Sprite[ ] variations;

	private SpriteRenderer spriteRenderer;

	void Start ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		spriteRenderer.sprite = variations[Utils.GetRandomInteger(0, variations.Length - 1)];
	}
}
