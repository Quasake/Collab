using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
	public bool isActive;
	public int id;

	public Sprite[ ] states;

	private Vector3 connectionPos;

	private SpriteRenderer spriteRenderer;

	void Start ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		spriteRenderer.sprite = states[isActive ? 1 : 0];
		connectionPos = transform.position + Vector3.down;
	}
}
