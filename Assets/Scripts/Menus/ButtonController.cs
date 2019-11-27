using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
	[Header("Sounds")]
	[SerializeField] AudioClip select = null;
	[SerializeField] AudioClip press = null;

	AudioSource audioSource;
	Animator anim;

	bool pressed;

	#region Unity Methods

	void Awake ( ) {
		audioSource = GetComponent<AudioSource>( );
		anim = GetComponent<Animator>( );
	}

	void Start ( ) {
		pressed = false;
	}

	void Update ( ) {
		if (!pressed) {
			if (anim.GetBool("Selected")) {
				Utils.PlaySound(audioSource, select);
			} else if (anim.GetBool("Pressed")) {
				pressed = true;

				Utils.PlaySound(audioSource, press);
			}
		}
	}

	#endregion
}
