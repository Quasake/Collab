using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
	[Header("Variables")]
	[SerializeField] private int groupID;
	[SerializeField] private bool isActive;

	[Header("Sprites")]
	[SerializeField] private Sprite[ ] states;

	private Vector3 connectionPos;

	private SpriteRenderer spriteRenderer;

	private void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		connectionPos = transform.position + Vector3.down;
	}

	private void Start ( ) {
		spriteRenderer.sprite = states[isActive ? 1 : 0];
	}
}
