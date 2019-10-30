using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {
	public int id;
	public bool isActive;
	public bool isBackground;
	public int formation; // 0 = short, 1 = straight, 2 = turn

	// 0 = off foreground, 1 = on foreground, 2 = off background, 3 = on background
	public Sprite[ ] shortVariations;
	public Sprite[ ] straightVariations;
	public Sprite[ ] turnVariations;
	Sprite[ ][ ] variations;

	private SpriteRenderer spriteRenderer;

	void Start ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
		variations = new Sprite[ ][ ] {
			shortVariations, straightVariations, turnVariations
		};

		spriteRenderer.sprite = variations[formation][(isBackground ? 2 : 0) + (isActive ? 1 : 0)];
		transform.localScale += Constants.GAP_FIX_NUMBER * new Vector3(1, 1);
	}
}
