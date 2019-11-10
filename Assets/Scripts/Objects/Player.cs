using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/Brackeys/2D-Character-Controller

public class Player : MonoBehaviour {
	[Header("Variables")]
	[SerializeField] int playerID; // The id of the player
	[SerializeField] int mode; // The power-up mode of the player
	[SerializeField] bool isDead; // Whether the player is deceased or not
	[SerializeField] bool isAtEnd; // Whether the player is at the end of the level or not
	[SerializeField] bool isEnabled; // Whether the player is enabled duh
	float jumpSpeed; // How high the player jumps
	float moveSpeed; // How fast the player moves
	float xMove; // How much the player should move each frame
	bool doJump; // Whether or not the player should jump
	bool isGrounded; // If the player is on the ground
	bool isSmall; // Whether the player is shrunk or not

	bool isModeSelect; // If the player is selecting a power-up mode
	int selectedMode; // The currently selected mode in the mode selection state

	[Space(20)]
	[SerializeField] Sprite[ ] tags; // The tags for the players so the players know which one is which
	[SerializeField] GameObject chunkPrefab; // The prefab for the player chunks
	[SerializeField] GameObject modeSelectObj; // An object that holds the mode selection state objects
	[SerializeField] LayerMask whatIsGround; // The object layer that is the ground duh
	[SerializeField] Transform groundCheck; // Ground collision detector
	[SerializeField] SpriteRenderer tagRenderer; // The player tag sprite renderer
	[SerializeField] Animator arrowAnimator; // The arrow animator thing idk these comments are getting annoying to write oof

	Rigidbody2D rBody2D; // Reference to the rigidbody of the player
	Animator animator; // Reference to the animator of the player
	SpriteRenderer spriteRenderer; // Reference to the sprite renderer of the player
	Collider2D coll2D; // Reference to the collider of the player

	Transform spawnpoint; // The position of the spawnpoint in the level
	GameManager gameManager; // The manager object of the game
	Transform objective; // The end of the level

	void Awake ( ) {
		rBody2D = GetComponent<Rigidbody2D>( );
		animator = GetComponent<Animator>( );
		coll2D = GetComponent<Collider2D>( );
		spriteRenderer = GetComponent<SpriteRenderer>( );

		spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>( );
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>( );
		objective = GameObject.Find("Objective").GetComponent<Transform>( );

		moveSpeed = Constants.PLAYER_DEF_MOVESPEED;
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;

		SetMode(Constants.PLAYER_NORM_MODE);
	}

	void Start ( ) {
		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;

		tagRenderer.sprite = tags[playerID];

		SetEnabled(true);
	}

	void Update ( ) {
		if (!isDead) {
			if (!isAtEnd) {
				if (!isModeSelect) {
					xMove = Utils.GetAxisRawValue("Horizontal", playerID) * moveSpeed;
					doJump = Utils.GetButtonValue("A", playerID);

					if (Utils.GetButtonValue("Start", playerID)) {
						Death( );
					}

					if (Utils.GetButtonValue("B", playerID)) {
						gameManager.Interact(coll2D);
					}

					if (Utils.GetButtonValue("X", playerID) && isGrounded) {
						if (mode == Constants.PLAYER_SHRINK_MODE) {
							isSmall = !isSmall;

							jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * ((isSmall) ? Constants.SHRINK_SMALL_AMOUNT : 1);
						} else if (mode == Constants.PLAYER_SWAP_MODE) {
							// DO THIS
						}
					}

					if (Utils.GetDistance(transform.position, objective.position) < 2f) {
						isAtEnd = true;
						xMove = 0;
						rBody2D.gravityScale = 0;
					}
				} else {
					float horiAxis = Utils.GetAxisRawValue("Horizontal", playerID);
					float vertAxis = Utils.GetAxisRawValue("Vertical", playerID);
					if (horiAxis > Constants.DEADZONE) { // Right
						selectedMode = Constants.PLAYER_BOOST_MODE;
					} else if (horiAxis < -Constants.DEADZONE) { // Left
						selectedMode = Constants.PLAYER_SWAP_MODE;
					} else if (vertAxis > Constants.DEADZONE) { // Up
						selectedMode = Constants.PLAYER_NORM_MODE;
					} else if (vertAxis < -Constants.DEADZONE) { // Down
						selectedMode = Constants.PLAYER_SHRINK_MODE;

						isSmall = false;
						animator.SetBool("isSmall", isSmall);
					}

					arrowAnimator.SetInteger("selectedMode", selectedMode);
				}

				if (Utils.GetButtonValue("Y", playerID)) {
					if (isModeSelect) { // If the mode was originally true, the player is in the mode selection screen
						SetMode(selectedMode); // Set the mode of the player
					} else { // If the mode was false, they are going into the mode selection screen
						xMove = 0; // Stop the player from moving
					}

					SetModeMenu(!isModeSelect);
				}
			} else if (isEnabled) {
				transform.position = Vector3.Lerp(transform.position, objective.position, Constants.PLAYER_SMOOTHING);
				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, Constants.TWO_PI * Mathf.Rad2Deg), Constants.PLAYER_SMOOTHING);

				if (Utils.AlmostEqual(transform.position, objective.position, 0.1f)) {
					SetEnabled(false);
				}
			}

			animator.SetFloat("xMove", Mathf.Abs(xMove));
			animator.SetBool("isJumping", !isGrounded);
			animator.SetBool("isSmall", isSmall);
			animator.SetBool("isAtEnd", isAtEnd);
		}
	}

	void FixedUpdate ( ) {
		Move( );
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Hazards")) {
			Death( );
		}
	}

	void Death ( ) {
		if (!isDead) {
			SetEnabled(false);

			for (int i = 0; i < Constants.CHUNK_COUNT; i++) {
				GameObject chunk = Instantiate(chunkPrefab, transform.position, new Quaternion(0, 0, Utils.GetRandomAngle( ), 0));
				chunk.GetComponent<Rigidbody2D>( ).velocity = Random.onUnitSphere * Utils.GetRandomFloat(Constants.CHUNK_FORCE_MIN, Constants.CHUNK_FORCE_MAX);
				chunk.GetComponent<Chunk>( ).SetType(mode);
			}

			transform.position = Constants.DEATH_POS;
			rBody2D.velocity = Vector3.zero;
			StartCoroutine(Respawn( ));

			isDead = true;
		}
	}

	void Move ( ) {
		// Check for ground
		Collider2D[ ] colliders = Physics2D.OverlapCircleAll(groundCheck.position, Constants.CHECK_RADIUS, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				isGrounded = true;
			}
		}

		// Move horizontally
		Vector3 targetVelocity = new Vector2(((!isModeSelect) ? xMove : 0) * Time.fixedDeltaTime * 10f, rBody2D.velocity.y);
		Vector3 zero = Vector3.zero;
		rBody2D.velocity = Vector3.SmoothDamp(rBody2D.velocity, targetVelocity, ref zero, Constants.PLAYER_SMOOTHING);

		// If grounded and the player wants to jump, then... well, jump
		if (isGrounded && doJump) {
			isGrounded = false;
			rBody2D.AddForce(new Vector2(0f, jumpSpeed));
		}

		// Reset jump so that you dont infinitly jump
		doJump = false;
	}

	void SetMode (int mode) {
		this.mode = mode;
		animator.SetInteger("mode", mode);

		// Set variables based on modes
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * ((mode == Constants.PLAYER_BOOST_MODE) ? Constants.BOOST_AMOUNT : 1);
	}

	void SetModeMenu (bool enabled) {
		isModeSelect = enabled;
		modeSelectObj.SetActive(enabled);
		spriteRenderer.sortingLayerName = enabled ? "Mini-UI" : "Player";
		tagRenderer.enabled = !enabled;
	}

	void SetEnabled (bool enabled) {
		isEnabled = enabled;

		rBody2D.isKinematic = !enabled;
		spriteRenderer.enabled = enabled;

		SetModeMenu(false);

		tagRenderer.enabled = enabled;
	}

	IEnumerator Respawn ( ) {
		float startTime = Time.time;

		while (Time.time - startTime <= Constants.PLAYER_RESPAWN_TIME) {
			yield return null;
		}

		SetEnabled(true);
		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
		isDead = false;
	}

	public bool GetIsDead ( ) {
		return isDead;
	}
}
