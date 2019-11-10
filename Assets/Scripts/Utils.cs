using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utils {
	public static bool AlmostEqual (Vector3 position, Vector3 target, float allowedDiff) {
		/* Get whether 2 positions are almost, but not exactly, equal */

		return GetDistance(position, target) <= allowedDiff;
	}

	public static float GetDistance (Vector2 point1, Vector2 point2) {
		/* Get the distance between 2 points */

		return Mathf.Sqrt(Mathf.Pow(point2.y - point1.y, 2) + Mathf.Pow(point2.x - point1.x, 2));
	}

	public static int GetRandomInteger (int min, int max) {
		/* Get a random integer between 2 values */

		return Constants.RANDOM.Next(min, max);
	}

	public static float GetRandomFloat (float min, float max) {
		/* Get a random float between 2 values */

		return (float) Constants.RANDOM.NextDouble( ) * (max - min) + min;
	}

	public static float GetRandomAngle ( ) {
		/* Get a random angle in radians */

		return GetRandomFloat(0, Constants.TWO_PI);
	}

	public static Vector3Int WorldPosToTilemapPos (Vector3 vector) {
		/* Convert an objects position in the world to a tile position on a tilemap */

		return new Vector3Int((int) (vector.x - 0.5f), (int) (vector.y - 0.5f), 0);
	}

	public static Vector3 GetMidpoint (Vector3 point1, Vector3 point2) {
		/* Get the midpoint between two points */

		float x = (point1.x + point2.x) / 2;
		float y = (point1.y + point2.y) / 2;

		return new Vector3(x, y);
	}

	public static void ChangeTileTexture (Tilemap tilemap, Vector3Int coord, Sprite sprite) {
		/* Set the texture of a tile in a tilemap */

		Tile tile = ScriptableObject.CreateInstance<Tile>( );
		tile.sprite = sprite;

		tilemap.SetTile(coord, tile);
	}

	public static Vector3Int Vec3ToVec3Int (Vector3 vector) {
		/* Convert a regular Vector3 to a Vector3Int */

		return new Vector3Int((int) vector.x, (int) vector.y, (int) vector.z);
	}

	public static float Limit (float value, float min, float max) {
		/* Limit a value to be inside a specific range */

		if (value < min) {
			value = min;
		}
		if (value > max) {
			value = max;
		}

		return value;
	}

	public static bool GetButtonValue (string name, int playerID) {
		/* Get joystick button value */

		return Input.GetButtonDown(name + "-" + (playerID + 1));
	}

	public static float GetAxisRawValue (string name, int playerID) {
		/* Get joystick axis value */

		return Input.GetAxisRaw(name + "-" + (playerID + 1));
	}

	public static bool InRange (float value, float min, float max) {
		/* Check if a value is within the specified range */

		return value >= min && value <= max;
	}

	public static bool InRangePM (float value, float range) {
		/* Check if a value is within the range made by <range> and -<range> */

		return InRange(value, -range, range);
	}

	public static Vector3 Rotate2D (Vector3 eulerAngles, float angle) {
		/* Get the 2D rotated euler angle vector based on the angle to rotate by */

		return new Vector3(eulerAngles.x, eulerAngles.y, Mathf.Rad2Deg * angle);
	}
}
