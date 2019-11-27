using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WireComponent : MonoBehaviour {
	[Header("Superclass [WireComponent]")]
	[SerializeField] protected int groupID;
	[SerializeField] protected bool isActive;
	[SerializeField] protected Connection connection;

	protected SpriteRenderer spriteRenderer;

	#region Unity Methods

	void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	#endregion

	#region Methods

	protected abstract void UpdateSprites ( );

	#endregion

	#region Setters

	public void SetActive (bool isActive) {
		this.isActive = isActive;
		if (connection != null) {
			connection.SetActive(isActive);
		}

		UpdateSprites( );
	}

	#endregion

	#region Getters

	public bool IsActive ( ) {
		return isActive;
	}

	public int GetID ( ) {
		return groupID;
	}

	#endregion
}
