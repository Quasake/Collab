using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public bool isExtended;
	public int id;

	public bool hasTopConnection;
	public bool hasBottomConnection;
	public bool isActiveTop;
	public bool isActiveBottom;
	
	public Sprite[ ] connections;

	private Vector3 topGroundConnectionPos;
	private Vector3 bottomGroundConnectionPos;

	public Transform door;
	public SpriteRenderer topSpriteRenderer;
	public SpriteRenderer bottomSpriteRenderer;

	void Start ( ) {
		topSpriteRenderer.sprite = connections[hasTopConnection ? (isActiveTop ? 1 : 0) : 2];
		bottomSpriteRenderer.sprite = connections[hasBottomConnection ? (isActiveBottom ? 1 : 0) : 2];
		topGroundConnectionPos = topSpriteRenderer.transform.position + Vector3.up;
		bottomGroundConnectionPos = bottomSpriteRenderer.transform.position + Vector3.down;
	}
}
