using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	[Header("Player")]
	[SerializeField] private Player player1;
	[SerializeField] private Player player2;

	private void Update ( ) {
		if (!player1.GetIsDead( ) || !player2.GetIsDead( )) {
			Vector3 player1Pos = (player1.GetIsDead( ) ? player2.transform.position : player1.transform.position);
			Vector3 player2Pos = (player2.GetIsDead( ) ? player1.transform.position : player2.transform.position);
			Vector3 toPos = Utils.GetMidpoint(player1Pos, player2Pos);
			toPos.z = transform.position.z;
			transform.position = Vector3.Lerp(transform.position, toPos, Constants.PLAYER_SMOOTHING);
		}
	}
}
