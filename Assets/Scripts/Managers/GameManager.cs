using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[Header("Environment")] // Environment GameObjects
	[SerializeField] Transform leverParent = null;
	[SerializeField] Transform doorParent = null;
	[SerializeField] Transform wireParent = null;
	[Header("Menus")]
	[SerializeField] PauseMenu pauseMenu = null;
	[SerializeField] InGameUI inGameUI = null;

	GameObject[ ] levers;
	GameObject[ ] doors;
	GameObject[ ] wires;

	static Player player1;
	static Player player2;
	Vector3 player1SwapPos;
	Vector3 player2SwapPos;
	bool isSwappingPlayers;

	#region Unity Methods

	void Awake ( ) {
		levers = Utils.GetAllChildren(leverParent);
		doors = Utils.GetAllChildren(doorParent);
		wires = Utils.GetAllChildren(wireParent);

		player1 = GameObject.Find("Player 1").GetComponent<Player>( );
		player2 = GameObject.Find("Player 2").GetComponent<Player>( );
	}

	void Update ( ) {
		if (isSwappingPlayers) {
			player1.transform.position = Vector3.Lerp(player1.transform.position, player2SwapPos, Constants.SMOOTHING);
			player2.transform.position = Vector3.Lerp(player2.transform.position, player1SwapPos, Constants.SMOOTHING);

			if (Utils.AlmostEqual(player1.transform.position, player2SwapPos, 0.05f) && Utils.AlmostEqual(player2.transform.position, player1SwapPos, 0.05f)) {
				isSwappingPlayers = false;

				player1.SetColliders(true);
				player2.SetColliders(true);
				player1.SetSortingLayer("Player");
				player2.SetSortingLayer("Player");
			}
		}
	}

	#endregion

	#region Methods

	#region Menu Methods

	public void Pause (int playerID) {
		pauseMenu.Pause(playerID);
	}

	public bool IsPaused ( ) {
		return pauseMenu.IsPaused( );
	}

	public bool IsOutOfMoves ( ) {
		return inGameUI.IsOutOfMoves( );
	}

	public void DecrementMoves ( ) {
		inGameUI.DecrementMoves( );
	}

	#endregion

	public void Interact (Collider2D collider) {
		for (int i = 0; i < levers.Length; i++) {
			if (collider.bounds.Intersects(levers[i].GetComponent<Collider2D>( ).bounds)) {
				Lever lever = levers[i].GetComponent<Lever>( );

				lever.Toggle( );
				SetWireGroup(lever.IsActive( ), lever.GetID( ));
			}
		}
	}

	public void SwapPlayers ( ) {
		isSwappingPlayers = true;

		player1.SetColliders(false);
		player2.SetColliders(false);
		player1.SetSortingLayer("Mini-UI");
		player2.SetSortingLayer("Mini-UI");

		player1SwapPos = player1.transform.position;
		player2SwapPos = player2.transform.position;
	}

	void SetWireGroup (bool isActive, int groupID) {
		for (int i = 0; i < wires.Length; i++) {
			Wire wire = wires[i].GetComponent<Wire>( );

			if (wire.GetID( ) == groupID) {
				wire.SetActive(isActive);
			}
		}

		for (int i = 0; i < doors.Length; i++) {
			Door door = doors[i].GetComponent<Door>( );

			if (door.GetID( ) == groupID) {
				door.SetActive(isActive);
			}
		}
	}

	#endregion

	#region Setters



	#endregion

	#region Getters

	public static Player GetPlayer1 ( ) {
		return player1;
	}

	public static Player GetPlayer2 ( ) {
		return player2;
	}

	public bool IsSwappingPlayers ( ) {
		return isSwappingPlayers;
	}

	#endregion
}
