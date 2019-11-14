using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	[SerializeField] Sprite[ ] normalChunks;
	[SerializeField] Sprite[ ] boostChunks;
	[SerializeField] Sprite[ ] shrinkChunks;
	[SerializeField] Sprite[ ] swapChunks;
	Sprite[ ][ ] chunks;

	SpriteRenderer spriteRenderer;

	float startTime; // The time that the chunk was spawned
	float disappearTime; // The lifetime of the chunk
	int type;

	void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		chunks = new Sprite[ ][ ] {
			normalChunks, boostChunks, shrinkChunks, swapChunks
		};
	}

	void Start ( ) {
		startTime = Time.time;
		disappearTime = Utils.GetRandomFloat(Constants.CHUNK_TIME_MIN, Constants.CHUNK_TIME_MAX);
		spriteRenderer.sprite = chunks[type][Utils.GetRandomInteger(0, Constants.CHUNK_COUNT)];
	}
	
	void Update ( ) {
		if (Time.time - startTime >= disappearTime) {
			StartCoroutine(FadeOut( ));
		}
	}

	IEnumerator FadeOut ( ) {
		/* Fade out the chunk and then destroy it after a certain amount of time */

		float startTime = Time.time;

		while (Time.time - startTime <= disappearTime / 2) {
			float ratio = (Time.time - startTime) / (disappearTime / 2);
			spriteRenderer.color = Color.Lerp(Constants.WHITE_FULL_ALPHA, Constants.WHITE_NO_ALPHA, ratio);

			yield return null;
		}

		spriteRenderer.color = Constants.WHITE_NO_ALPHA;

		Destroy(gameObject);
	}

	public void SetType (int type) {
		this.type = type;
	}
}
