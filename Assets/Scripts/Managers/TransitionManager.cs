using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {
	Animator anim;

	int sceneToLoad = 0; // The scene to load after transition

	#region Unity Methods

	void Awake ( ) {
		anim = GetComponent<Animator>( );
	}

	#endregion

	#region Methods

	#region Public

	public void RestartScene ( ) {
		FadeToScene(SceneManager.GetActiveScene( ).buildIndex);
	}

	public void FadeToScene (int sceneToLoad) {
		this.sceneToLoad = sceneToLoad;

		anim.SetTrigger("fade");
	}

	#endregion

	#region Private

	void LoadScene ( ) {
		SceneManager.LoadScene(sceneToLoad);
	}

	#endregion

	#endregion
}
