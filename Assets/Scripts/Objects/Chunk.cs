using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	SpriteRenderer spriteRenderer;

	float startTime;
	float disappearTime;

	void Awake ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );
	}

	void Start ( ) {
		startTime = Time.time;
		disappearTime = Utils.GetRandomFloat(Constants.CHUNK_TIME_MIN, Constants.CHUNK_TIME_MAX);
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
}
