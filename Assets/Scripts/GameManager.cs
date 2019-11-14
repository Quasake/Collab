using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] Transform leverParent;
	[SerializeField] Transform doorParent;
	[SerializeField] Transform wireParent;
	[Header("Level")]
	[SerializeField] int numMoves;
	[Header("Children")]
	[SerializeField] Text remainingMoves;
	[SerializeField] Canvas pauseMenu;
	[SerializeField] Canvas completedMenu;
	[SerializeField] Canvas inGameMenu;
	[SerializeField] GameObject firstPauseButton;
	[SerializeField] GameObject firstCompletedButton;

	bool isOutOfMoves;
	bool isCompleted;
	bool isPaused;
	int playerPaused;

	List<Lever> levers;
	List<Door> doors;
	List<Wire> wires;

	Player player1;
	Player player2;

	EventSystem eventSystem;
	StandaloneInputModule inputModule;

	void Start ( ) {
		levers = new List<Lever>( );
		doors = new List<Door>( );
		wires = new List<Wire>( );

		player1 = GameObject.Find("Player 1").GetComponent<Player>( );
		player2 = GameObject.Find("Player 2").GetComponent<Player>( );

		eventSystem = EventSystem.current;
		inputModule = eventSystem.GetComponent<StandaloneInputModule>( );

		for (int i = 0; i < leverParent.childCount; i++) {
			levers.Add(leverParent.GetChild(i).GetComponent<Lever>( ));
		}
		for (int i = 0; i < doorParent.childCount; i++) {
			doors.Add(doorParent.GetChild(i).GetComponent<Door>( ));
		}
		for (int i = 0; i < wireParent.childCount; i++) {
			wires.Add(wireParent.GetChild(i).GetComponent<Wire>( ));
		}

		inGameMenu.enabled = true;
		completedMenu.enabled = false;
		pauseMenu.enabled = false;
	}

	void Update ( ) {
		remainingMoves.text = numMoves + "";

		if (player1.isAtEnd && player2.isAtEnd) {
			if (!isCompleted) {
				SetInputs(Constants.PLAYER_1_ID, firstCompletedButton);

				completedMenu.enabled = true;
				inGameMenu.enabled = false;
				isCompleted = true;
			}
		}
	}

	public void Interact (Collider2D collider) {
		for (int i = 0; i < levers.Count; i++) {
			if (collider.bounds.Intersects(levers[i].GetComponent<Collider2D>( ).bounds)) {
				levers[i].Toggle( );
				SetWireGroup(levers[i].GetIsActive( ), levers[i].GetID( ));
			}
		}
	}

	public void DecrementMoves ( ) {
		if (!isOutOfMoves) {
			numMoves--;
			if (numMoves == 0) {
				isOutOfMoves = true;
			}
		}
	}

	public void TogglePause (int playerID) {
		if (!isCompleted) {
			isPaused = !isPaused;
			playerPaused = playerID;

			SetInputs(playerPaused, firstPauseButton);

			pauseMenu.enabled = isPaused;
			inGameMenu.enabled = !isPaused;
		}
	}

	void SetInputs (int playerID, GameObject firstObject) {
		playerID++;
		inputModule.horizontalAxis = "Horizontal-" + playerID;
		inputModule.verticalAxis = "Vertical-" + playerID;
		inputModule.submitButton = "A-" + playerID;
		inputModule.cancelButton = "B-" + playerID;

		eventSystem.SetSelectedGameObject(firstObject);
	}

	void SetWireGroup (bool isActive, int groupID) {
		for (int i = 0; i < wires.Count; i++) {
			if (wires[i].GetID( ) == groupID) {
				wires[i].SetIsActive(isActive);
			}
		}
		for (int i = 0; i < doors.Count; i++) {
			if (doors[i].GetID( ) == groupID) {
				doors[i].SetIsActive(isActive);
			}
		}
	}
}
