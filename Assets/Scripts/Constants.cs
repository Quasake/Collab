using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
	public static System.Random RANDOM = new System.Random( );

	public static Vector3 SPAWNPOINT_OFFSET = new Vector3(0, 1);
	
	public const float GAP_FIX_NUMBER = 0.0001f;

	public const float PLAYER_JUMPSPEED = 600f;
	public const float PLAYER_SMOOTHING = 0.05f;
	public const float PLAYER_MOVESPEED = 30f;

	public const float CHECK_RADIUS = 0.2f;
	public const float DEADZONE = 0.05f;

	public const int MODE_NORMAL = 0;
	public const int MODE_BOOST = 1;
	public const int MODE_SHRINK = 2;
	public const int MODE_SWAP = 3;
}
