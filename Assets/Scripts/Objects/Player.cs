using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/Brackeys/2D-Character-Controller

public class Player : MonoBehaviour {
	public int id; // The id of the player
	int mode; // The power-up mode of the player
	float jumpSpeed; // How high the player jumps
	float moveSpeed; // How fast the player moves
	float xMove; // How much the player should move each frame
	bool jump; // Whether or not the player should jump
	bool isGrounded; // If the player is on the ground
	bool dead; // Whether the player is deceased or not

	bool modeSelection; // If the player is selecting a power-up mode
	int selectedMode; // The currently selected mode in the mode selection state
	bool modeChanged; // Whether the player has changed the mode that was highlighted in the mode selection state

	// Arrays for the chunks for each of the player modes
	public Sprite[ ] normalChunks;
	public Sprite[ ] boostChunks;
	public Sprite[ ] shrinkChunks;
	public Sprite[ ] swapChunks;
	Sprite[ ][ ] chunks;

	public Transform groundCheck; // Ground collision detector
	public GameObject modeSelectionObject; // An object that holds the mode selection state objects
	public GameObject chunkPrefab; // The prefab for the player chunks
	Transform modeSelectionArrow; // The arrow in the mode selection state
	public LayerMask whatIsGround; // The object layer that ground is

	Transform spawnpoint; // The position of the spawnpoint in the level
	Rigidbody2D body; // Reference to the rigidbody of the player
	Animator animator; // Reference to the animator of the player
	SpriteRenderer spriteRenderer; // Reference to the sprite renderer of the player

	void Start ( ) {
		spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>( );
		body = GetComponent<Rigidbody2D>( );
		animator = GetComponent<Animator>( );
		spriteRenderer = GetComponent<SpriteRenderer>( );
		modeSelectionArrow = modeSelectionObject.transform.GetChild(0);
		chunks = new Sprite[ ][ ] {
			normalChunks, boostChunks, shrinkChunks, swapChunks
		};

		jumpSpeed = Constants.PLAYER_JUMPSPEED;
		moveSpeed = Constants.PLAYER_MOVESPEED;

		transform.position = spawnpoint.position + Constants.SPAWNPOINT_OFFSET;
	}

	void Update ( ) {
		if (!modeSelection) {
			xMove = Input.GetAxisRaw("Horizontal-" + (id + 1)) * moveSpeed;

			if (Input.GetButtonDown("A-" + (id + 1))) {
				jump = true;
			}
		} else {
			ModeSelection( );
		}

		if (Input.GetButtonDown("Y-" + (id + 1))) {
			if (modeSelection) {
				SetMode(selectedMode);
			} else {
				xMove = 0;
			}

			modeSelection = !modeSelection;
			modeSelectionObject.SetActive(modeSelection);
		}

		animator.SetFloat("xMove", Mathf.Abs(xMove));
		animator.SetBool("isJumping", !isGrounded);
	}

	void ModeSelection ( ) {
		if (!modeChanged) {
			if (Input.GetAxisRaw("Horizontal-" + (id + 1)) > Constants.DEADZONE) {
				selectedMode = Constants.MODE_BOOST;
				modeChanged = true;
			} else if (Input.GetAxisRaw("Horizontal-" + (id + 1)) < -Constants.DEADZONE) {
				selectedMode = Constants.MODE_SWAP;
				modeChanged = true;
			} else if (Input.GetAxisRaw("Vertical-" + (id + 1)) > Constants.DEADZONE) {
				selectedMode = Constants.MODE_NORMAL;
				modeChanged = true;
			} else if (Input.GetAxisRaw("Vertical-" + (id + 1)) < -Constants.DEADZONE) {
				selectedMode = Constants.MODE_SHRINK;
				modeChanged = true;
			}

			if (modeChanged) {
				modeSelectionArrow.transform.eulerAngles = new Vector3(modeSelectionArrow.transform.eulerAngles.x, modeSelectionArrow.transform.eulerAngles.y, -90 * selectedMode);
				modeSelectionArrow.transform.localPosition = new Vector3((selectedMode % 2 == 1) ? (selectedMode - 2) * -0.75f : 0, (selectedMode % 2 == 0) ? (selectedMode - 1) * -0.75f : 0);
			}
		} else {
			if (Input.GetAxisRaw("Horizontal-" + (id + 1)) < Constants.DEADZONE && Input.GetAxisRaw("Horizontal-" + (id + 1)) > -Constants.DEADZONE) {
				modeChanged = false;
			}
		}
	}

	void FixedUpdate ( ) {
		CheckGround( );

		Move( );

		jump = false;
	}

	private void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.name.Equals("Spike")) {
			Death( );
		}
	}

	void Death ( ) {
		spriteRenderer.enabled = false;

		StartCoroutine(_SpawnChunks( ));
	}

	IEnumerator _SpawnChunks ( ) {
		for (int i = 0; i < Constants.CHUNK_NUM; i++) {
			GameObject chunk = Instantiate(chunkPrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
			chunk.transform.SetParent(transform, false);
			chunk.GetComponent<Rigidbody2D>( ).AddForce(new Vector2())
		}

		yield return null;
	}

	void CheckGround ( ) {
		Collider2D[ ] colliders = Physics2D.OverlapCircleAll(groundCheck.position, Constants.CHECK_RADIUS, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				isGrounded = true;
			}
		}
	}

	void Move ( ) {
		Vector3 targetVelocity = new Vector2(((!modeSelection) ? xMove : 0) * Time.fixedDeltaTime * 10f, body.velocity.y);
		Vector3 zero = Vector3.zero;
		body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref zero, Constants.PLAYER_SMOOTHING);

		if (isGrounded && jump) {
			isGrounded = false;
			body.AddForce(new Vector2(0f, jumpSpeed));
		}
	}

	void SetMode (int mode) {
		this.mode = mode;

		animator.SetInteger("mode", mode);

		jumpSpeed = Constants.PLAYER_JUMPSPEED * ((mode == Constants.MODE_BOOST) ? Constants.BOOST_AMOUNT : 1);
	}
}
