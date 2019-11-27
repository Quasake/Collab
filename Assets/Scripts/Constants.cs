using UnityEngine;

/*	ORDER OF SERIALIZATION:

	0. Superclass
	1. Variables
	2. Children
	3. Environment
	4. Sprites
	5. Sounds
	4. Other

*/

#region Unity Methods

#endregion

#region Methods

#endregion

#region Coroutines

#endregion

#region Setters

#endregion

#region Getters

#endregion

public static class Constants {
	public static string PATH_LEVELS = Application.persistentDataPath + "/levels.collab";

	public static System.Random RANDOM = new System.Random( );

	public static Vector3 SPAWNPOINT_OFFSET = new Vector3(0, 0.5f);
	public static Vector3 DEATH_POS = new Vector3(0, 40);

	public const float CHECK_RADIUS = 0.05f;
	public const float DEADZONE = 0.05f;
	public const float SMOOTHING = 0.05f;

	public const int MENU_TITLESCREEN = 0;
	public const int MENU_LEVELSELECT = 1;
	public const int MENU_OPTIONS = 2;
	public const int MENU_CREDTIS = 3;

	public const int NUM_LEVELS = 10;

	public const int PLAYER_1_ID = 0;
	public const int PLAYER_2_ID = 1;
	public const float PLAYER_DEF_JUMPSPEED = 600f;
	public const float PLAYER_DEF_MOVESPEED = 30f;
	public const float PLAYER_RESPAWN_TIME = 3f;
	public const int PLAYER_NORM_MODE = 0;
	public const int PLAYER_BOOST_MODE = 1;
	public const int PLAYER_SHRINK_MODE = 2;
	public const int PLAYER_SWAP_MODE = 3;

	public const float CHUNK_TIME_MIN = 2f;
	public const float CHUNK_TIME_MAX = 4f;
	public const float CHUNK_FORCE_MIN = 20f;
	public const float CHUNK_FORCE_MAX = 30f;
	public const int CHUNK_COUNT = 6;

	public const float CANNON_SPAWNRATE = 0.2f;

	public const float BOOST_AMOUNT = 1.4f;
	public const float SMALL_AMOUNT = 0.6f;

	public static Color WHITE_FULL_ALPHA = new Color(1f, 1f, 1f, 1f);
	public static Color WHITE_NO_ALPHA = new Color(1f, 1f, 1f, 0f);

	public const float DOOR_MOVESPEED = 0.1f;

	public const float OBJECTIVE_DIST = 2f;

	public const float CAMERA_ZOOM_LIMITER = 50f;
	public const float CAMERA_ZOOM_MIN = 4f;
	public const float CAMERA_ZOOM_MAX = 8f;
}
