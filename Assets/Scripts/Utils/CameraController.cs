using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField] bool canMove = true;
	[SerializeField] Player[ ] players = null;

	Camera cam;

	Vector3 velocity;

	void Update ( ) {
		cam = GetComponent<Camera>( );
	}

	void LateUpdate ( ) {
		if (players.Length > 0 && canMove) {
			Move( );
			Zoom( );
		}
	}

	void Move ( ) {
		Vector3 move = Utils.NoZ(GetBounds( ).center, transform.position.z);

		transform.position = Vector3.SmoothDamp(transform.position, move, ref velocity, 0.5f);
	}

	void Zoom ( ) {
		float zoom = Mathf.Lerp(Constants.CAMERA_ZOOM_MIN, Constants.CAMERA_ZOOM_MAX, Mathf.Max(GetBounds( ).size.x, GetBounds( ).size.y) / 20f);

		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime);
	}

	Bounds GetBounds ( ) {
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		bool foundAlivePlayer = false;

		for (int i = 0; i < players.Length; i++) {
			if (!players[i].IsDead( )) {
				if (!foundAlivePlayer) {
					bounds = new Bounds(players[i].transform.position, Vector3.zero);
					foundAlivePlayer = true;
				} else {
					bounds.Encapsulate(players[i].transform.position);
				}
			}
		}

		return bounds;
	}
}