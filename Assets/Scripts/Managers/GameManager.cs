using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[Header("Children")]
	[SerializeField] PauseMenu pauseMenu = null;
	[SerializeField] InGameUI inGameUI = null;

	static Player player1;
	static Player player2;

	GameObject[ ] levers;
	GameObject[ ] doors;
	GameObject[ ] wires;

	Vector3 player1SwapPos;
	Vector3 player2SwapPos;
	Vector3 velocity1;
	Vector3 velocity2;

	bool isSwappingPlayers;

	#region Unity Methods

	void Awake ( ) {
		levers = Utils.GetAllChildren(GameObject.Find("Levers").GetComponent<Transform>( ));
		doors = Utils.GetAllChildren(GameObject.Find("Doors").GetComponent<Transform>( ));
		wires = Utils.GetAllChildren(GameObject.Find("Wires").GetComponent<Transform>( ));

		player1 = GameObject.Find("Player 1").GetComponent<Player>( );
		player2 = GameObject.Find("Player 2").GetComponent<Player>( );
	}

	void Update ( ) {
		if (isSwappingPlayers) {
			player1.SetPosition(Vector3.SmoothDamp(player1.GetPosition( ), player2SwapPos, ref velocity1, 0.5f));
			player2.SetPosition(Vector3.SmoothDamp(player2.GetPosition( ), player1SwapPos, ref velocity2, 0.5f));

			if (Utils.AlmostEqual(player1.GetPosition( ), player2SwapPos, Constants.CHECK_RADIUS) && Utils.AlmostEqual(player2.GetPosition( ), player1SwapPos, Constants.CHECK_RADIUS)) {
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

	#region Public

	#region Menu Methods

	public void Pause (int playerID) {
		pauseMenu.Pause(playerID);
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
		if (player1.IsAble( ) && player2.IsAble( )) {
			isSwappingPlayers = true;

			player1.SetColliders(false);
			player2.SetColliders(false);
			player1.SetSortingLayer("Mini-UI");
			player2.SetSortingLayer("Mini-UI");

			player1SwapPos = player1.GetPosition( );
			player2SwapPos = player2.GetPosition( );
		}
	}

	#endregion

	#region Private

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

	#endregion

	#region Getters

	public bool IsPaused ( ) {
		return pauseMenu.IsPaused( );
	}

	public bool IsOutOfMoves ( ) {
		return inGameUI.IsOutOfMoves( );
	}

	public bool IsAble ( ) {
		return !IsPaused( ) && !IsOutOfMoves( );
	}

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
