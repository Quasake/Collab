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
	[Header("Children")]
	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject completedMenu;
	[SerializeField] GameObject inGameUI;
	[SerializeField] GameObject pauseFirstObject;
	[SerializeField] GameObject completedFirstObject;
	[Header("Level")]
	[SerializeField] int numMoves;
	[SerializeField] Text remainingMoves;

	public bool isPaused;
	public bool isCompleted;
	bool isOutOfMoves;
	int playerPaused;

	List<Lever> levers;
	List<Door> doors;
	List<Wire> wires;

	Player player1;
	Player player2;

	EventSystem eventSystem;
	StandaloneInputModule inputModule;

	void Awake ( ) {
		levers = new List<Lever>( );
		doors = new List<Door>( );
		wires = new List<Wire>( );

		player1 = GameObject.Find("Player 1").GetComponent<Player>( );
		player2 = GameObject.Find("Player 2").GetComponent<Player>( );

		eventSystem = EventSystem.current;
		inputModule = eventSystem.GetComponent<StandaloneInputModule>( );
	}

	void Start ( ) {
		for (int i = 0; i < leverParent.childCount; i++) {
			levers.Add(leverParent.GetChild(i).GetComponent<Lever>( ));
		}
		for (int i = 0; i < doorParent.childCount; i++) {
			doors.Add(doorParent.GetChild(i).GetComponent<Door>( ));
		}
		for (int i = 0; i < wireParent.childCount; i++) {
			wires.Add(wireParent.GetChild(i).GetComponent<Wire>( ));
		}

		completedMenu.SetActive(false);
		pauseMenu.SetActive(false);
		inGameUI.SetActive(true);
	}

	void Update ( ) {
		remainingMoves.text = numMoves + "";

		if (player1.isAtEnd && player2.isAtEnd) {
			if (!isCompleted) {
				isCompleted = true;
				completedMenu.SetActive(true);
				inGameUI.SetActive(false);

				if (isCompleted) {
					SetInputs(Constants.PLAYER_1_ID, completedFirstObject);
				}
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

	public void TogglePause (int playerID) {
		isPaused = !isPaused;
		playerPaused = playerID;

		pauseMenu.SetActive(isPaused);
		inGameUI.SetActive(!isPaused);

		if (isPaused) {
			SetInputs(playerID, pauseFirstObject);
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

	void SetInputs (int playerID, GameObject firstButton) {
		playerID++;

		inputModule.horizontalAxis = "Horizontal-" + playerID;
		inputModule.verticalAxis = "Vertical-" + playerID;
		inputModule.submitButton = "A-" + playerID;
		inputModule.cancelButton = "B-" + playerID;

		eventSystem.SetSelectedGameObject(firstButton, new BaseEventData(eventSystem));
	}
}
