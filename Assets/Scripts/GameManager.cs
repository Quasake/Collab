using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] private Transform leverParent;
	[SerializeField] private Transform doorParent;
	[SerializeField] private Transform wireParent;

	[Header("Level")]
	[SerializeField] int numMoves;

	List<Lever> levers;
	List<Door> doors;
	List<Wire> wires;

	void Start ( ) {
		levers = new List<Lever>( );
		doors = new List<Door>( );
		wires = new List<Wire>( );

		for (int i = 0; i < leverParent.childCount; i++) {
			levers.Add(leverParent.GetChild(i).GetComponent<Lever>( ));
		}
		for (int i = 0; i < doorParent.childCount; i++) {
			doors.Add(doorParent.GetChild(i).GetComponent<Door>( ));
		}
		for (int i = 0; i < wireParent.childCount; i++) {
			wires.Add(wireParent.GetChild(i).GetComponent<Wire>( ));
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
