using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Component {
	[Header("Environment")]
	[SerializeField] private GameManager gameManager;

	public bool isConnectedTop;

	// 0 = off, 1 = on, 2 = none
	public Sprite[ ] variations;

	public Transform door;
	public SpriteRenderer topSpriteRenderer;
	public SpriteRenderer bottomSpriteRenderer;

	private void Awake ( ) {
		topSpriteRenderer.transform.localScale += Constants.GAP_FIX_NUMBER * new Vector3(1, 1);
		bottomSpriteRenderer.transform.localScale += Constants.GAP_FIX_NUMBER * new Vector3(1, 1);
	}

	protected override void UpdateSprites ( ) {
		topSpriteRenderer.sprite = variations[isConnectedTop ? (isActive ? 1 : 0) : 2];
		bottomSpriteRenderer.sprite = variations[isConnectedTop ? 2 : (isActive ? 1 : 0)];

		gameManager.SetTileTexture(this, isActive);
	}

	private void Update ( ) {
		Vector3 doorMove = (isActive ? -1 : 1) * new Vector3(0, (isConnectedTop ? 1 : -1) * Constants.DOOR_MOVESPEED);
		door.position = new Vector3(door.position.x, Utils.Limit(door.position.y + doorMove.y, -1.5f, 1.5f));
		Debug.Log(door.position.y + doorMove.y);
	}
}
