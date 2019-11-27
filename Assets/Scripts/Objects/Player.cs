using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://github.com/Brackeys/2D-Character-Controller

public class Player : MonoBehaviour {
	[Header("Variables")] // Player variables
	[SerializeField] int playerID = -1;
	[SerializeField] int mode = Constants.PLAYER_NORM_MODE;
	[SerializeField] bool isDead = false;
	[SerializeField] bool isAtEnd = false;
	[SerializeField] bool canChangeBoost = false;
	[SerializeField] bool canChangeShrink = false;
	[SerializeField] bool canChangeSwap = false;
	[Header("Children")] // Children GameObjects
	[SerializeField] Transform groundCheck = null;
	[SerializeField] ModeSelectionMenu modeSelectMenu = null;
	[Header("Environment")] // Environment GameObjects
	[SerializeField] GameObject chunkPref = null;
	[SerializeField] LayerMask whatIsGround = -1;
	[Header("Sounds")]
	[SerializeField] AudioClip jump = null;
	[SerializeField] AudioClip death = null;
	[SerializeField] AudioClip changePower = null;
	[SerializeField] AudioClip end = null;

	Rigidbody2D rBody2D;
	Animator anim;
	SpriteRenderer spriteRenderer;
	Collider2D coll2D;
	AudioSource audioSource;

	GameManager gameManager;
	Transform spawnpoint;
	Transform objective;

	float jumpSpeed;
	float xMove;
	bool doJump;
	bool isGrounded;
	bool isSmall;
	bool isEnabled;

	#region Unity Methods

	void Awake ( ) {
		rBody2D = GetComponent<Rigidbody2D>( );
		anim = GetComponent<Animator>( );
		coll2D = GetComponent<Collider2D>( );
		audioSource = GetComponent<AudioSource>( );
		spriteRenderer = GetComponent<SpriteRenderer>( );

		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>( );
		spawnpoint = GameObject.Find("Spawnpoint " + (playerID + 1)).transform;
		objective = GameObject.Find("Objective").transform;
	}

	void Start ( ) {
		jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED;
		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;

		SetMode(Constants.PLAYER_NORM_MODE);
		SetEnabled(true);
	}

	void Update ( ) {
		if (!isDead) {
			if (!isAtEnd) {
				if (!modeSelectMenu.IsEnabled( ) && !gameManager.IsSwappingPlayers( )) {
					UpdateInput( ); // Update input
					CheckAtEnd( ); // Check if the player has reached the end of the level
				}

				// Check for ground
				Collider2D[ ] colliders = Physics2D.OverlapCircleAll(groundCheck.position, Constants.CHECK_RADIUS, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders[i].gameObject != gameObject) {
						isGrounded = true;
					}
				}
			} else if (isEnabled) {
				transform.position = Vector3.Lerp(transform.position, objective.position, Time.deltaTime * 3);
				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 360), Time.deltaTime * 3);

				if (Utils.AlmostEqual(transform.position, objective.position, Constants.OBJECTIVE_DIST / 20f)) {
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
		if (Utils.GetDistance(transform.position, objective.position) < Constants.OBJECTIVE_DIST) {
			isAtEnd = true;
			rBody2D.gravityScale = 0;
			rBody2D.velocity = Vector3.zero;

			// Utils.PlaySound(audioSource, end);
		}
	}

	void Death ( ) {
		if (!isDead) {
			Utils.PlaySound(audioSource, death);

			// Disable the player
			SetEnabled(false);

			// Spawn in the chunks
			for (int i = 0; i < Constants.CHUNK_COUNT; i++) {
				GameObject chunk = Instantiate(chunkPref, transform.position, new Quaternion(0, 0, Utils.GetRandomAngle( ), 0));

				chunk.GetComponent<Rigidbody2D>( ).velocity = Random.onUnitSphere * Utils.GetRandomFloat(Constants.CHUNK_FORCE_MIN, Constants.CHUNK_FORCE_MAX);
				chunk.GetComponent<Chunk>( ).SetType(mode);
				// chunk.transform.SetParent(transform, true);
			}

			transform.position = Constants.DEATH_POS;
			isDead = true;

			StartCoroutine(Respawn( ));
		}
	}

	void Move ( ) {
		// Move horizontally
		Vector3 targetVelocity = new Vector2(xMove * Time.fixedDeltaTime * 10f, rBody2D.velocity.y);
		Vector3 zero = Vector3.zero;
		rBody2D.velocity = Vector3.SmoothDamp(rBody2D.velocity, targetVelocity, ref zero, Constants.SMOOTHING);

		// If grounded and the player wants to jump, then... well, jump
		if (isGrounded && doJump) {
			isGrounded = false;
			rBody2D.AddForce(new Vector2(0f, jumpSpeed));

			Utils.PlaySound(audioSource, jump);
		}

		// Reset moving variables so the player doesnt move indefinitely
		doJump = false;
		xMove = 0;
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
					jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * ((isSmall) ? Constants.SMALL_AMOUNT : 1);
				} else if (mode == Constants.PLAYER_SWAP_MODE) {
					gameManager.SwapPlayers( );
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

		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
		isDead = false;

		SetEnabled(true);
	}

	#endregion

	#region Setters

	public void SetMode (int mode) {
		if (this.mode != mode) {
			Utils.PlaySound(audioSource, changePower);

			this.mode = mode;
			anim.SetInteger("mode", mode);

			// Set variables based on modes
			isSmall = false;
			jumpSpeed = Constants.PLAYER_DEF_JUMPSPEED * ((mode == Constants.PLAYER_BOOST_MODE) ? Constants.BOOST_AMOUNT : 1);

			gameManager.DecrementMoves( );
		}
	}

	public void SetColliders (bool isEnabled) {
		rBody2D.isKinematic = !isEnabled;

		Collider2D[ ] colls = GetComponentsInChildren<Collider2D>( );
		for (int i = 0; i < colls.Length; i++) {
			colls[i].isTrigger = !isEnabled;
		}
	}

	public void SetSortingLayer (string layer) {
		spriteRenderer.sortingLayerName = layer;
	}

	void SetEnabled (bool isEnabled) {
		this.isEnabled = isEnabled;

		spriteRenderer.enabled = isEnabled;

		SetColliders(isEnabled);

		if (!isEnabled) {
			modeSelectMenu.SetEnabled(false);
		}
		modeSelectMenu.SetTagEnabled(isEnabled);
	}

	public void SetPosition (Vector3 newPos) {
		transform.position = newPos;
	}

	#endregion

	#region Getters

	public Vector3 GetPosition ( ) {
		return transform.position;
	}

	public bool IsAble ( ) {
		return !isDead && !isAtEnd;
	}

	public bool IsDead ( ) {
		return isDead;
	}

	public bool IsAtEnd ( ) {
		return isAtEnd;
	}

	public int GetID ( ) {
		return playerID;
	}

	public int GetMode ( ) {
		return mode;
	}

	public bool CanChangeBoost ( ) {
		return canChangeBoost;
	}

	public bool CanChangeShrink ( ) {
		return canChangeShrink;
	}

	public bool CanChangeSwap ( ) {
		return canChangeSwap;
	}

	#endregion
}
