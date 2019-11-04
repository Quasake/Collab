﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
	public static System.Random RANDOM = new System.Random( );

	public static Vector3 SPAWNPOINT_OFFSET = new Vector3(0, 0.5f);
	public static Vector3 DEATH_POS = new Vector3(0, 20);

	public const float GAP_FIX_NUMBER = 0.0001f;
	public const float CHECK_RADIUS = 0.2f;
	public const float DEADZONE = 0.05f;

	public const int PLAYER_1_ID = 0;
	public const int PLAYER_2_ID = 1;
	public const float PLAYER_DEF_JUMPSPEED = 600f;
	public const float PLAYER_DEF_MOVESPEED = 30f;
	public const float PLAYER_RESPAWN_TIME = 3f;
	public const float PLAYER_SMOOTHING = 0.05f;
	public const int PLAYER_NORM_MODE = 0;
	public const int PLAYER_BOOST_MODE = 1;
	public const int PLAYER_SHRINK_MODE = 2;
	public const int PLAYER_SWAP_MODE = 3;

	public const float CHUNK_TIME_MIN = 3f;
	public const float CHUNK_TIME_MAX = 5f;
	public const float CHUNK_FORCE_MIN = 20f;
	public const float CHUNK_FORCE_MAX = 30f;
	public const int CHUNK_COUNT = 6;

	public const float BOOST_AMOUNT = 1.4f;

	public static Color WHITE_FULL_ALPHA = new Color(1f, 1f, 1f, 1f);
	public static Color WHITE_NO_ALPHA = new Color(1f, 1f, 1f, 0f);

	public const float HALF_PI = Mathf.PI / 2;
	public const float PI = Mathf.PI;
	public const float THREE_FOURTH_PI = Mathf.PI * 3 / 4;
	public const float TWO_PI = Mathf.PI * 2;

	public const float DOOR_MOVESPEED = 0.1f;

	public const int UP = 0;
	public const int RIGHT = 1;
	public const int DOWN = 2;
	public const int LEFT = 3;
}
