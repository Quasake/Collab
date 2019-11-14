using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/Brackeys/2D-Character-Controller

public class Player : MonoBehaviour {
	float jumpSpeed;
	float xMove;
	bool doJump;
	bool isGrounded;
	bool isSmall;
	bool isModeSelect;
	int selectedMode;

	[Header("Variables")]
	[SerializeField] int playerID;
	[SerializeField] int mode;
	public bool isDead;
	public bool isAtEnd;
	[Header("Sprites")]
	[SerializeField] Sprite[ ] tags;
	[Header("Environment")]
	[SerializeField] GameObject chunkPrefab;
	[SerializeField] LayerMask whatIsGround;
	[Header("Children")]
	[SerializeField] Transform groundCheck;
	[SerializeField] SpriteRenderer tagRenderer;
	[SerializeField] GameObject modeSelectObj;
	[SerializeField] Animator arrowAnimator;

	Rigidbody2D rBody2D;
	Animator animator;
	SpriteRenderer spriteRenderer;
	Collider2D coll2D;

	Transform spawnpoint;
	GameManager gameManager;
	Transform objective;

	void Awake ( ) {
		rBody2D = GetComponent<Rigidbody2D>( );
		animator = GetComponent<Animator>( );
		coll2D = GetComponent<Collider2D>( );
		spriteRenderer = GetComponent<SpriteRenderer>( );

		spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>( );
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>( );
		objective = GameObject.Find("Objective").GetComponent<Transform>( );
	}

	void Start ( ) {
		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
		tagRenderer.sprite = tags[playerID];
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;

		SetMode(Constants.PLAYER_NORM_MODE);
		SetEnabled(true);
	}

	void Update ( ) {
		if (!isDead) {
			if (!isAtEnd) {
				if (!isModeSelect) {
					UpdateInput( );

					if (Utils.GetDistance(transform.position, objective.position) < Constants.OBJECTIVE_DIST) {
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
				transform.position = Vector3.Lerp(transform.position, objective.position, Constants.PLAYER_SMOOTHING);
				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, Constants.TWO_PI * Mathf.Rad2Deg), Constants.PLAYER_SMOOTHING);

				if (Utils.AlmostEqual(transform.position, objective.position, 0.1f)) {
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

	void UpdateAnimator ( ) {
		animator.SetFloat("xMove", Mathf.Abs(xMove));
		animator.SetBool("isJumping", !isGrounded);
		animator.SetBool("isSmall", isSmall);
		animator.SetBool("isAtEnd", isAtEnd);
	}

	void UpdateInput ( ) {
		xMove = Utils.GetAxisRawValue("Horizontal", playerID) * Constants.PLAYER_DEF_MOVESPEED;
		doJump = Utils.GetButtonValue("A", playerID);

		if (Utils.GetButtonValue("Select", playerID)) {
			Death( );
		}

		if (Utils.GetButtonValue("Start", playerID)) {
			gameManager.TogglePause(playerID);
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

	IEnumerator Respawn ( ) {
		float startTime = Time.time;

		while (Time.time - startTime <= Constants.PLAYER_RESPAWN_TIME) {
			yield return null;
		}

		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
		isDead = false;

		SetEnabled(true);
	}

	void SetMode (int mode) {
		this.mode = mode;
		animator.SetInteger("mode", mode);

		// Set variables based on modes
		if (mode == Constants.PLAYER_NORM_MODE) {
			jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;
		} else if (mode == Constants.PLAYER_BOOST_MODE) {
			jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * Constants.BOOST_AMOUNT;
		} else if (mode == Constants.PLAYER_SHRINK_MODE) {
			jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;
		} else if (mode == Constants.PLAYER_SWAP_MODE) {
			jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;
		}
	}

	void SetTagEnabled (bool enabled) {
		tagRenderer.enabled = enabled;
	}

	void SetModeMenu (bool enabled) {
		isModeSelect = enabled;
		modeSelectObj.SetActive(enabled);
		spriteRenderer.sortingLayerName = enabled ? "Mini-UI" : "Player";

		SetTagEnabled(!enabled);
	}

	void SetEnabled (bool enabled) {
		rBody2D.isKinematic = !enabled;
		spriteRenderer.enabled = enabled;
		coll2D.isTrigger = !enabled;

		SetModeMenu(false);
		SetTagEnabled(enabled);
	}

	public bool IsDead ( ) {
		return isDead;
	}
}
