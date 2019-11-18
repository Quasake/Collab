using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour {
	Animator anim;

	int sceneToLoad = 0; // The scene to load after transition

	void Awake ( ) {
		anim = GetComponent<Animator>( );
	}

	public void RestartScene ( ) {
		FadeToScene(SceneManager.GetActiveScene( ).name);
	}

	public void FadeToScene (string scene) {
		sceneToLoad = SceneManager.GetSceneByName(scene).buildIndex;

		anim.SetTrigger("fade");
	}

	void LoadScene ( ) {
		SceneManager.LoadScene(sceneToLoad);
	}
}
