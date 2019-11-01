using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Component : MonoBehaviour {
	[Header("Variables")]
	[SerializeField] protected int groupID;
	[SerializeField] protected bool isActive;
	[SerializeField] protected Vector3 connectionPos;

	private void Start ( ) {
		SetIsActive(isActive);
	}

	protected abstract void UpdateSprites ();

	public void SetIsActive (bool isActive) {
		this.isActive = isActive;

		UpdateSprites( );
	}

	public bool GetIsActive ( ) {
		return isActive;
	}

	public int GetID ( ) {
		return groupID;
	}

	public Vector3 GetConnectionPos ( ) {
		return connectionPos;
	}
}
