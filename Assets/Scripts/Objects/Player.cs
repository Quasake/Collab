using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int id;
	public int mode;

	private Transform spawnpoint;

	void Start ( ) {
		spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>( );

		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
	}
}
