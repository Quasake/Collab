using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	[Header("Player")]
	[SerializeField] private Player player1;
	[SerializeField] private Player player2;
	[SerializeField] private Transform spawnpoint;

	private void Start ( ) {
		transform.position = new Vector3(spawnpoint.position.x, spawnpoint.position.y, transform.position.z);
	}

	private void Update ( ) {
		Vector3 toPos = spawnpoint.position;
		if (!player1.IsDead( ) || !player2.IsDead( )) {
			Vector3 player1Pos = (player1.IsDead( ) ? player2.transform.position : player1.transform.position);
			Vector3 player2Pos = (player2.IsDead( ) ? player1.transform.position : player2.transform.position);
			toPos = Utils.GetMidpoint(player1Pos, player2Pos);
		}

		toPos.z = transform.position.z;
		transform.position = Vector3.Lerp(transform.position, toPos, Constants.PLAYER_SMOOTHING);
	}
}
