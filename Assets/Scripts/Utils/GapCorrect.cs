using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapCorrect : MonoBehaviour {
	[SerializeField] Transform[ ] objects = null;

	void Start ( ) {
		for (int i = 0; i < objects.Length; i++) {
			objects[i].localScale += Constants.GAP_FIX;
		}
	}
}
