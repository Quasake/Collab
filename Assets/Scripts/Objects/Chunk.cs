using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	float startTime;

	SpriteRenderer spriteRenderer;

	void Start ( ) {
		spriteRenderer = GetComponent<SpriteRenderer>( );

		startTime = Time.time;
	}
	
	void Update ( ) {
		if (Time.time - startTime >= Constants.CHUNK_TIME) {
			StartCoroutine(_Fade( ));
		}

		if (spriteRenderer.color.a == 0) {
			Destroy(gameObject);
		}
	}

	IEnumerator _Fade ( ) {
		float ratio = (Time.time - startTime) / (Constants.CHUNK_TIME / 2);

		while (ratio < 1) {
			spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, ratio));

			yield return null;
		}

		spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
	}
}
