using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public int facingDirection; // right = 1, left = -1
	[HideInInspector] public bool jumpTrigger;
	public float moveSpeed;
	public float acceleration;
	public float currentMoveSpeed;
	public float jumpForce;
	public float wallSlidingSpeed;
	public int jumpCount;
	public int maxJumpCount;

	public Rigidbody2D rb2d;
	public BoxCollider2D boxCollider;
	public LayerMask groundMask;



	private int numberOfRaycast;
	private RaycastHit2D[] bottomHits;
	private RaycastHit2D[] aheadHits;
	private float colliderSizeX;
	private float colliderSizeY;

	public bool grounded;

	public enum CharacterState {Idle, Running, Jumping, Falling, Sliding};
	public CharacterState charState;





	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		numberOfRaycast = 4;
		bottomHits = new RaycastHit2D[numberOfRaycast];
		aheadHits = new RaycastHit2D[numberOfRaycast];
		colliderSizeX = boxCollider.size.x * transform.localScale.x;
		colliderSizeY = boxCollider.size.y * transform.localScale.y;
		rb2d.velocity = Vector2.down * 10f;



	}

	void FixedUpdate()
	{
		InputControl ();

		RayHitUpdate ();

		StateUpdate ();

		if (jumpTrigger) {
			Jump ();
		}

		MoveControl ();



	}




	void InputControl()
	{
		if (Mathf.Abs(Input.GetAxis("Horizontal")) != 0) {
			rb2d.AddForce (Vector2.right * Input.GetAxis ("Horizontal") * Time.fixedDeltaTime * 100f);
		}

		if (Input.GetButtonDown("Jump"))
		{
			jumpTrigger = true;
		}

	}



	void Jump()
	{
		rb2d.AddForce (Vector2.up * jumpForce);
		jumpTrigger = false;
	}

	void MoveControl()
	{
		//Handle whenever MC collide with ground
		RaycastHit2D shortestBottomHit = new RaycastHit2D();
		foreach (var hit in bottomHits) {
			if (hit) {
				if (!shortestBottomHit) {
					shortestBottomHit = hit;
				} else {
					if (shortestBottomHit.distance > hit.distance) {
						shortestBottomHit = hit;
					}
				}
			}
		}
		float verticalVelocity = rb2d.velocity.y;
		if (shortestBottomHit) {
			Debug.DrawLine ((Vector3)shortestBottomHit.point + Vector3.up * shortestBottomHit.distance, (Vector3)shortestBottomHit.point, Color.cyan);
			float minorGap = verticalVelocity * Time.fixedDeltaTime - (shortestBottomHit.distance - colliderSizeY / 2);
			if (minorGap > 0) {
				/// set MC position collide with the ground
			}
				
			// MOVE IT TO StateUpdate()
			if (shortestBottomHit.distance - colliderSizeY / 2 < 0.001f) {
				grounded = true;
			} else {
				grounded = false;
			}
		}




	}



	void StateUpdate()
	{
		/*
		// ground check
		for (int i = 0; i < bottomHits.Length; i++) {

			if (bottomHits[i]) {
				//print (string.Format("{0} {1}", hit, hit.distance));
				if (Mathf.Abs(bottomHits[i].distance - colliderSizeY/2) < 0.01f) {
					//print (string.Format("{0} {1}", i, bottomHits[i].distance));
				}
			}
		}
		*/

	}


	void RayHitUpdate()
	{

		Vector2 boxColliderOffset = boxCollider.offset;
		/// bottom cast
		for (int i = 0; i < numberOfRaycast; i++) {
			int middleIndex = numberOfRaycast / 2;
			if (numberOfRaycast % 2 == 1) {
				middleIndex = numberOfRaycast + 1;
			}

			Vector2 startPosition = (Vector2)transform.position + boxColliderOffset + new Vector2 (-boxCollider.size.x / 2 + boxCollider.size.x / (numberOfRaycast - 1) * i, 0);
			float castDistance = boxCollider.size.y / 2 * 5f;

			bottomHits [i] = Physics2D.Raycast (startPosition, Vector2.down, castDistance, groundMask);
			if (bottomHits[i]) {
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)bottomHits[i].point, Color.cyan);
			} else {
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)startPosition + Vector3.down * castDistance, Color.cyan);
			}
		}


		/// ahead cast
		for (int i = 0; i < numberOfRaycast; i++) {
			int middleIndex = numberOfRaycast / 2;
			if (numberOfRaycast % 2 == 1) {
				middleIndex = numberOfRaycast + 1;
			}

			Vector2 startPosition = (Vector2)transform.position + boxColliderOffset + new Vector2 (0, -boxCollider.size.y / 2 + boxCollider.size.y / (numberOfRaycast - 1) * i);
			float castDistance = boxCollider.size.x / 2 * 5f;
			aheadHits [i] = Physics2D.Raycast (startPosition, Vector2.right, castDistance, groundMask);
			if (aheadHits[i]) {
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)aheadHits[i].point, Color.cyan);
			} else {
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)startPosition + Vector3.right * castDistance, Color.cyan);
			}
		}









	}

}