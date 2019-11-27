using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCannon : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] GameObject chunkPref = null;

	float timer;

	#region Unity Methods

	void Start ( ) {
		timer = Time.time;
	}

	void Update ( ) {
		if (Time.time - timer >= Constants.CANNON_SPAWNRATE) {
			timer = Time.time;

			GameObject chunk = Instantiate(chunkPref, transform.position, new Quaternion(0, 0, Utils.GetRandomAngle( ), 0));

			float randAngle = (transform.eulerAngles.z + Utils.GetAngle(-10, 10)) * Mathf.Deg2Rad;
			Vector2 randVel = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));

			chunk.GetComponent<Rigidbody2D>( ).velocity = randVel * Utils.GetRandomFloat(Constants.CHUNK_FORCE_MIN, Constants.CHUNK_FORCE_MAX);
			chunk.GetComponent<Chunk>( ).SetType(Utils.GetRandomInteger(Constants.PLAYER_NORM_MODE, Constants.PLAYER_SWAP_MODE));
			chunk.transform.localScale *= 3;
			chunk.transform.SetParent(transform, true);
		}
	}

	#endregion
}
