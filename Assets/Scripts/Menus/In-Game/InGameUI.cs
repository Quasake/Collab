using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
	[Header("Variables")]
	[SerializeField] int movesLeft = 0;
	[Header("Children")]
	[SerializeField] Text movesText = null;

	#region Unity Methods

	void Update ( ) {
		movesText.text = movesLeft + "";
	}

	#endregion

	#region Methods

	public void DecrementMoves ( ) {
		if (movesLeft > 0) {
			movesLeft--;
		}
	}

	public bool IsOutOfMoves ( ) {
		return movesLeft == 0;
	}

	#endregion
}
