using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
	[SerializeField] int movesLeft = 0;
	[SerializeField] Text movesText = null;

	void Update ( ) {
		movesText.text = movesLeft + "";
	}

	public void DecrementMoves ( ) {
		if (movesLeft > 0) {
			movesLeft--;
		}

		if (IsOutOfMoves( )) {
			GameManager.GetPlayer1( ).SetModeMenu(false);
			GameManager.GetPlayer2( ).SetModeMenu(false);
		}
	}

	public bool IsOutOfMoves ( ) {
		return movesLeft == 0;
	}
}
