using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Component : MonoBehaviour {
	[Header("Component Class")]
	[SerializeField] protected int groupID;
	[SerializeField] protected bool isActive;
	[SerializeField] protected Connection connection;
	protected SpriteRenderer spriteRenderer;

	private void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	protected abstract void UpdateSprites ( );

	public void SetIsActive (bool isActive) {
		this.isActive = isActive;
		if (connection != null) {
			connection.SetIsActive(isActive);
		}

		UpdateSprites( );
	}

	public bool GetIsActive ( ) {
		return isActive;
	}

	public int GetID ( ) {
		return groupID;
	}
}
