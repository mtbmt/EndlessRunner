using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	public int facingDirection; // right = 1, left = -1
	[HideInInspector] public bool jumpTrigger;
	public float moveSpeed;
	public float acceleration;
	public float currentMoveSpeed;
	public float jumpForce;
	public float wallSlidingSpeed;
	public int jumpCount;
	public int maxJumpCount;

	public bool collidingBottom;
	public ArrayList collidingBottomPoint;
	public bool collidingLeft;
	public ArrayList collidingLeftPoint;
	public bool collidingRight;
	public ArrayList collidingRightPoint;

	public bool grounded;
	public bool justGrounded;
	public bool climbing;
	public bool justClimbed;
	public bool jumping;
	private bool justJumpTrigged;
	private bool justJumped;
	public bool onCorner;
	private float mcCircleColliderRadius;
	public float gravityValue;
	public LayerMask groundMask;
	private Rigidbody2D rb2d;
	void Awake()
	{
		mcCircleColliderRadius = GetComponent<CircleCollider2D> ().radius;
		rb2d = GetComponent<Rigidbody2D>();
		jumpCount = 0;
		facingDirection = 1;
		jumpTrigger = false;
	}

	void Update() 
	{
		if (Input.GetButtonDown("Jump"))
		{
			jumpTrigger = true;
		}
	}

	void FixedUpdate()
	{
		StateUpdate ();
		ParameterUpdate ();
		MoveControl ();
		if (jumpTrigger) {
			Jump ();
		}
		currentMoveSpeed = Mathf.Abs(rb2d.velocity.x);
	}




	void Jump()
	{
		jumpTrigger = false;
		if (jumpCount < maxJumpCount) {
			justJumpTrigged = true;
			if (grounded) {
				if (onCorner) {
					rb2d.isKinematic = false;
					//Flip
					facingDirection = - facingDirection;
					rb2d.AddForce(new Vector2(jumpForce * facingDirection,jumpForce));
				} else {
					rb2d.velocity = new Vector2 (facingDirection * moveSpeed, rb2d.velocity.y);
					rb2d.AddForce(new Vector2(0f, jumpForce));
				}

			} else {

				//Flip
				facingDirection = - facingDirection;
				rb2d.velocity = new Vector2 (facingDirection * moveSpeed, 0);
				rb2d.AddForce(new Vector2(0f, jumpForce));
			}
		}


	}


	void MoveControl()
	{
		Vector2 moveForce = Vector2.zero;
		//calculate all forces when on left wall
		if (collidingLeft && climbing) {
			Vector2 rayCast = (Vector2)collidingLeftPoint [collidingLeftPoint.Count - 1] - (Vector2)transform.position;
			if (justClimbed) {
				rb2d.velocity = Vector3.zero;
			}
			moveForce = rayCast / rayCast.magnitude * gravityValue;
			if (rb2d.velocity.magnitude < wallSlidingSpeed) {
				moveForce = moveForce + Vector2.down * gravityValue * wallSlidingSpeed / moveSpeed;
			}
		}
		//calculate all forces when on the right wall
		if (collidingRight && climbing) {
			Vector2 rayCast = (Vector2)collidingRightPoint [collidingRightPoint.Count - 1] - (Vector2)transform.position;
			if (justClimbed) {
				rb2d.velocity = Vector3.zero;
			}
			moveForce = rayCast / rayCast.magnitude * gravityValue;
			if (rb2d.velocity.magnitude < wallSlidingSpeed) {
				moveForce = moveForce + Vector2.down * gravityValue * wallSlidingSpeed / moveSpeed;
			}
		}
		//calculate all forces when on the ground
		if (collidingBottom) {
			foreach (var collidingPoint in collidingBottomPoint) {
				Vector2 rayCast = (Vector2)collidingPoint - (Vector2)transform.position;
				moveForce = rayCast.normalized * gravityValue;
			}

		}

		//calculate velocity on the air
		if (!climbing && !onCorner && !grounded) {
			moveForce = Vector2.down * gravityValue;
			/*
			 * no need to set fixed velocity in the air
			 * REMOVED
			if (Mathf.Abs(Mathf.Abs(rb2d.velocity.x) - moveSpeed) > 0.1f) {
				rb2d.velocity = new Vector2(facingDirection*moveSpeed, rb2d.velocity.y);
			}
			*/
		}


		//calculate velocity on the ground
		if (grounded) {
			/*
			 // set velocity
			Vector2 rayCast = (Vector2)collidingBottomPoint [collidingBottomPoint.Count - 1] - (Vector2)transform.position;
			Vector2 direction = new Vector2 (facingDirection, 0);
			Vector2 currentDirection = Quaternion.AngleAxis (90, Vector3.forward) * rayCast;
			if (Vector2.Angle(currentDirection, direction) >= 90) {
				currentDirection = Quaternion.AngleAxis (-90, Vector3.forward) * rayCast;
			}
			Vector2 currentVelocity = currentDirection / currentDirection.magnitude * moveSpeed;
			if (Mathf.Abs(rb2d.velocity.magnitude - moveSpeed)> 0.1f) {
				rb2d.velocity = currentVelocity;
			}

			*/

			// add force to mc
			foreach (var collidingPoint in collidingBottomPoint) {
				Vector2 rayCast = (Vector2)collidingPoint - (Vector2)transform.position;
				Vector2 direction = new Vector2 (facingDirection, 0);
				Vector2 currentDirection = Quaternion.AngleAxis (90, Vector3.forward) * rayCast;
				if (Vector2.Angle(currentDirection, direction) >= 90) {
					currentDirection = Quaternion.AngleAxis (-90, Vector3.forward) * rayCast;
				}

				if (rb2d.velocity.magnitude < moveSpeed) {
					rb2d.AddForce (currentDirection.normalized * acceleration);
				}
			}


		}

		// Add force
		rb2d.AddForce (moveForce);

		if (onCorner) {
			rb2d.isKinematic = true;
		}

	}



	void StateUpdate()
	{

		// reset status
		collidingBottom = false;
		collidingBottomPoint = new ArrayList ();
		collidingLeft = false;
		collidingLeftPoint = new ArrayList ();
		collidingRight = false;
		collidingRightPoint = new ArrayList ();

		// Detect collision: right, left, bottm
		RaycastHit2D[] allCircleCast = Physics2D.CircleCastAll (transform.position, mcCircleColliderRadius * 1.1f, Vector3.zero, 0, groundMask);
		foreach (var hit in allCircleCast) {
			Vector2 rayDirection = (Vector2)hit.point - (Vector2)transform.position;
			RaycastHit2D rayCast = Physics2D.Raycast (transform.position, rayDirection, mcCircleColliderRadius * 1.1f, groundMask);
			//Draw all contact points
			Debug.DrawLine (transform.position, rayCast.point, Color.red);
			float rayAngle = Vector2.Angle (rayDirection, Vector2.down);
			if (rayAngle <= 50) {
				collidingBottom = true;
				collidingBottomPoint.Add (rayCast.point);
			} else if (rayAngle <= 135) {
				if (rayDirection.x < 0) {
					collidingLeft = true;
					collidingLeftPoint.Add (rayCast.point);
				} else {
					collidingRight = true;
					collidingRightPoint.Add (rayCast.point);
				}
			}

		}


		//Update State

		if ((collidingLeft || collidingRight) && collidingBottom) {
			onCorner = true;
		} else {
			onCorner = false;
		}

		if ((collidingLeft || collidingRight) && !collidingBottom) {
			if (climbing == false) {
				justClimbed = true;
			} else {
				justClimbed = false;
			}
			climbing = true;

		} else {
			climbing =false;
		}

		if (collidingBottom) {
			if (grounded == false) {
				justGrounded = true;
			} else {
				justGrounded = false;
			}
			grounded = true;
		} else {
			grounded = false;
		}

		if (justJumpTrigged && rb2d.velocity.y > 0) {
			jumping = true;
			justJumpTrigged = false;
			justJumped = true;
		} else if (rb2d.velocity.y < 0) {
			jumping = false;
		}

	}



	void ParameterUpdate()
	{
		if (justJumped) {
			jumpCount += 1;
			justJumped = false;
		}	
		if (grounded) {
			jumpCount = 0;
		}


	}
}
