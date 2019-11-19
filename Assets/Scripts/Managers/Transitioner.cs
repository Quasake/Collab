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
		FadeToScene(SceneManager.GetActiveScene( ).buildIndex);
	}

	public void FadeToScene (int sceneToLoad) {
		this.sceneToLoad = sceneToLoad;

		anim.SetTrigger("fade");
	}

	void LoadScene ( ) {
		SceneManager.LoadScene(sceneToLoad);
	}
}
