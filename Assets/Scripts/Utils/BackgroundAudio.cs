using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour {
	static GameObject instance;

	#region Unity Methods

	void Awake ( ) {
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = gameObject;

			DontDestroyOnLoad(gameObject);
		}
	}

	#endregion
}
