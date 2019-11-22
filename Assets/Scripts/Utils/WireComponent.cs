using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WireComponent : MonoBehaviour {
	[Header("Component [Class]")]
	[SerializeField] protected int groupID;
	[SerializeField] protected bool isActive;
	[SerializeField] protected Connection connection;

	protected SpriteRenderer spriteRenderer;

	void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	protected abstract void UpdateSprites ( );

	public void SetActive (bool isActive) {
		this.isActive = isActive;
		if (connection != null) {
			connection.SetActive(isActive);
		}

		UpdateSprites( );
	}

	public bool IsActive ( ) {
		return isActive;
	}

	public int GetID ( ) {
		return groupID;
	}
}
