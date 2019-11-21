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
	static Transform spawnpoint;
	static Transform objective;

	#region Unity Methods

	void Awake ( ) {
		levers = Utils.GetAllChildren(leverParent);
		doors = Utils.GetAllChildren(doorParent);
		wires = Utils.GetAllChildren(wireParent);

		player1 = GameObject.Find("Player 1").GetComponent<Player>( );
		player2 = GameObject.Find("Player 2").GetComponent<Player>( );
		spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>( );
		objective = GameObject.Find("Objective").GetComponent<Transform>( );
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

	public static Transform GetSpawnpoint ( ) {
		return spawnpoint;
	}

	public static Transform GetObjective ( ) {
		return objective;
	}

	public static Player GetPlayer1 ( ) {
		return player1;
	}

	public static Player GetPlayer2 ( ) {
		return player2;
	}

	#endregion
}
