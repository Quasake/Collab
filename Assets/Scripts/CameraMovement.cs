using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] Transform spawnpoint = null;
	[Header("Player")]
	[SerializeField] Player player1 = null;
	[SerializeField] Player player2 = null;

	void Start ( ) {
		transform.position = new Vector3(spawnpoint.position.x, spawnpoint.position.y, transform.position.z);
	}

	void Update ( ) {
		Vector3 player1Pos = (player1.IsDead( ) ? player2.transform.position : player1.transform.position);
		Vector3 player2Pos = (player2.IsDead( ) ? player1.transform.position : player2.transform.position);

		Vector3 toPos = spawnpoint.position;
		if (!player1.IsDead( ) || !player2.IsDead( )) {
			toPos = Utils.GetMidpoint(player1Pos, player2Pos);
		}

		toPos.z = transform.position.z;
		transform.position = Vector3.Lerp(transform.position, toPos, Constants.PLAYER_SMOOTHING);

		float toZoom = Mathf.Lerp(Constants.CAMERA_ZOOM_MIN, Constants.CAMERA_ZOOM_MAX, Utils.GetHorizontalDistance(player1Pos, player2Pos) / Constants.CAMERA_ZOOM_LIMITER);
		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, toZoom, Time.deltaTime);
	}
}