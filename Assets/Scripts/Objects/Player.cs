using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/Brackeys/2D-Character-Controller

public class Player : MonoBehaviour {
	[Header("Variables")] // Player variables
	[SerializeField] int playerID = -1;
	[SerializeField] int mode = Constants.PLAYER_NORM_MODE;
	[SerializeField] bool isDead = false;
	[SerializeField] bool isAtEnd = false;
	[SerializeField] bool canChangeJump = false;
	[SerializeField] bool canChangeShrink = false;
	[SerializeField] bool canChangeSwap = false;
	[Header("Sprites")] // Sprites for the player
	[SerializeField] Sprite[ ] tags = null;
	[Header("Environment")] // Environment GameObjects
	[SerializeField] GameObject chunkPref = null;
	[SerializeField] LayerMask whatIsGround = -1;
	[Header("Children")] // Children GameObjects
	[SerializeField] Transform groundCheck = null;
	[SerializeField] SpriteRenderer tagRenderer = null;
	[SerializeField] GameObject modeSelectObj = null;
	[SerializeField] Animator arrowAnim = null;

	float jumpSpeed;
	float xMove;
	bool doJump;
	bool isGrounded;
	bool isSmall;
	bool isModeSelect;
	int selectedMode;

	Rigidbody2D rBody2D;
	Animator anim;
	SpriteRenderer spriteRenderer;
	Collider2D coll2D;
	GameManager gameManager;

	#region Unity Methods

	void Awake ( ) {
		rBody2D = GetComponent<Rigidbody2D>( );
		anim = GetComponent<Animator>( );
		coll2D = GetComponent<Collider2D>( );
		spriteRenderer = GetComponent<SpriteRenderer>( );

		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>( );
	}

	void Start ( ) {
		tagRenderer.sprite = tags[playerID];
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;
		transform.position = GameManager.GetSpawnpoint( ).position + Constants.SPAWNPOINT_OFFSET;

		SetMode(Constants.PLAYER_NORM_MODE);
		SetEnabled(true);
	}

	void Update ( ) {
		if (!isDead) {
			if (!isAtEnd) {
				if (!isModeSelect) {
					UpdateInput( ); // Update Input
					CheckAtEnd( ); // Check if the player has reached the end of the level
				} else {
					if (gameManager.IsOutOfMoves( )) {
						SetModeMenu(false);
					}

					UpdateModeSelection( ); // Update the mode selection menu
				}

				// If the game is not paused, not out of moves, and the Y button is pressed
				if (!gameManager.IsPaused( ) && !gameManager.IsOutOfMoves( ) && Utils.GetButtonValue("Y", playerID)) {
					if (isModeSelect) {
						if (selectedMode != mode) {
							gameManager.DecrementMoves( );
						}

						SetMode(selectedMode);
					} else {
						xMove = 0;
					}

					SetModeMenu(!isModeSelect);
				}
			} else {
				transform.position = Vector3.Lerp(transform.position, GameManager.GetObjective( ).position, Time.deltaTime * 3);
				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 360), Time.deltaTime * 3);

				if (Utils.AlmostEqual(transform.position, GameManager.GetObjective( ).position, Constants.OBJECTIVE_DIST / 20f)) {
					SetEnabled(false);
				}
			}

			UpdateAnimator( );
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

	#endregion

	#region Methods

	void CheckAtEnd ( ) {
		if (Utils.GetDistance(transform.position, GameManager.GetObjective( ).position) < Constants.OBJECTIVE_DIST) {
			isAtEnd = true;

			xMove = 0;
			rBody2D.gravityScale = 0;

			SetModeMenu(false);
		}
	}

	void UpdateModeSelection ( ) {
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
			anim.SetBool("isSmall", isSmall);
		}

		arrowAnim.SetInteger("selectedMode", selectedMode);
	}

	void Death ( ) {
		if (!isDead) {
			// Disable the player
			SetEnabled(false);

			// Spawn in the chunks
			for (int i = 0; i < Constants.CHUNK_COUNT; i++) {
				GameObject chunk = Instantiate(chunkPref, transform.position, new Quaternion(0, 0, Utils.GetRandomAngle( ), 0));

				chunk.GetComponent<Rigidbody2D>( ).velocity = Random.onUnitSphere * Utils.GetRandomFloat(Constants.CHUNK_FORCE_MIN, Constants.CHUNK_FORCE_MAX);
				chunk.GetComponent<Chunk>( ).SetType(mode);
				chunk.transform.SetParent(transform, true);
			}

			transform.position = Constants.DEATH_POS;

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

		// Reset jump so that the player don't infinitly jump
		doJump = false;
	}

	void UpdateAnimator ( ) {
		anim.SetFloat("xMove", Mathf.Abs(xMove));
		anim.SetBool("isJumping", !isGrounded);
		anim.SetBool("isSmall", isSmall);
		anim.SetBool("isAtEnd", isAtEnd);
	}

	void UpdateInput ( ) {
		if (!gameManager.IsPaused( )) {
			xMove = Utils.GetAxisRawValue("Horizontal", playerID) * Constants.PLAYER_DEF_MOVESPEED;
			doJump = Utils.GetButtonValue("A", playerID);

			if (Utils.GetButtonValue("Select", playerID)) {
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
		}

		if (Utils.GetButtonValue("Start", playerID)) {
			gameManager.Pause(playerID);
		}
	}

	IEnumerator Respawn ( ) {
		float startTime = Time.time;

		while (Time.time - startTime <= Constants.PLAYER_RESPAWN_TIME) {
			yield return null;
		}

		transform.position = GameManager.GetSpawnpoint( ).position + Constants.SPAWNPOINT_OFFSET;
		isDead = false;

		SetEnabled(true);
	}

	#endregion

	#region Setters

	void SetMode (int mode) {
		this.mode = mode;
		anim.SetInteger("mode", mode);

		// Set variables based on modes
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * ((mode == Constants.PLAYER_BOOST_MODE) ? Constants.BOOST_AMOUNT : 1);
	}

	void SetTagEnabled (bool enabled) {
		tagRenderer.enabled = enabled;
	}

	public void SetModeMenu (bool enabled) {
		isModeSelect = enabled;

		modeSelectObj.SetActive(enabled);
		spriteRenderer.sortingLayerName = enabled ? "Mini-UI" : "Player";

		SetTagEnabled(!enabled);
	}

	void SetEnabled (bool enabled) {
		rBody2D.isKinematic = !enabled;
		spriteRenderer.enabled = enabled;
		coll2D.isTrigger = !enabled;
		rBody2D.velocity = Vector3.zero;

		SetModeMenu(false);
		SetTagEnabled(enabled);
	}

	#endregion

	#region Getters

	public bool IsDead ( ) {
		return isDead;
	}

	public bool IsAtEnd ( ) {
		return isAtEnd;
	}

	#endregion
}
