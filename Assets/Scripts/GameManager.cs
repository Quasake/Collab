using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
	[Header("Environment")]
	[SerializeField] private Transform leverParent;
	[SerializeField] private Transform doorParent;
	[SerializeField] private Transform wireParent;

	[Header("Tilemap Connections")]
	[SerializeField] private Tilemap groundMap;
	[SerializeField] private Sprite[ ] topConnections;
	[SerializeField] private Sprite[ ] rightConnections;
	[SerializeField] private Sprite[ ] bottomConnections;
	[SerializeField] private Sprite[ ] leftConnections;
	private Sprite[ ][ ] connections;

	private List<Lever> levers;
	private List<Door> doors;
	private List<Wire> wires;

	private void Awake ( ) {
		connections = new Sprite[ ][ ] {
			topConnections, rightConnections, bottomConnections, leftConnections
		};
	}

	private void Start ( ) {
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

	public void SetWireGroup (bool isActive, int groupID) {
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

	public void SetTileTexture (Component component, bool isActive) {
		int formation = (int) ((Mathf.Deg2Rad * (component.transform.rotation.z + (component is Door ? 270 : 0))) / Constants.HALF_PI);
		Vector3Int tileCoord = Utils.WorldPosToTilemapPos(component.transform.position + component.GetConnectionPos());

		Utils.ChangeTileTexture(groundMap, tileCoord, connections[formation][isActive ? 1 : 0]);
	}
}
