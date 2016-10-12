using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {


	public int facingDirection; // right = 1, left = -1
	[HideInInspector] public bool jumpTrigger;
	public int jumpCount;
	public bool grounded;
	public bool climbing;
	public float moveSpeed;
	public float jumpForce;
	public float wallSlidingSpeed;
	public bool collidingLeft;
	public float collidingLeftAngle;
	public bool collidingRight;
	public float collidingRightAngle;
	public bool onCorner;


	private float kinematicCheckingTime;

	private Rigidbody2D rb2d;

	void Awake() 
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = new Vector2 (moveSpeed, rb2d.velocity.y);
		jumpCount = 0;
		facingDirection = 1;
		jumpTrigger = false;
		grounded = false;
		kinematicCheckingTime = Time.time;

	}

	// Update is called once per frame
	void Update() 
	{

		if (Input.GetButtonDown("Jump"))
		{
			jumpTrigger = true;
		}

			
	}
	void FixedUpdate()
	{
		CharacterStateUpdate ();
		//MoveControl ();

		if (jumpTrigger)
		{
			Jump ();
		}




	}


	void CharacterStateUpdate()
	{



		LayerMask layer = LayerMask.GetMask ("ground");
		float mcRadius = gameObject.GetComponent<CircleCollider2D> ().radius;




		RaycastHit2D botHit = Physics2D.CircleCast (transform.position, mcRadius, Vector2.down, mcRadius * 0.3f, layer.value);
		RaycastHit2D rightHit = Physics2D.CircleCast(transform.position, mcRadius, Vector2.right, mcRadius * 0.3f, layer.value);
		RaycastHit2D circleToLeft = Physics2D.CircleCast(transform.position, mcRadius, Vector2.left, mcRadius * 0.3f, layer.value);

		if (botHit) {
			Debug.DrawLine (transform.position, botHit.point, Color.red);
			float collidePointToDownAngle = Vector2.Angle (botHit.point - (Vector2)transform.position, Vector2.down);

			if (collidePointToDownAngle <= 60 && Vector2.Distance(transform.position, rightHit.point) + Time.deltaTime *moveSpeed <= mcRadius) {
				grounded = true;
			} else {
				collidingRight = false;
			}

		} else {
			grounded = false;
		}

		if (rightHit) {
			Debug.DrawLine (transform.position, rightHit.point, Color.blue);
			float collidePointToDownAngle = Vector2.Angle (rightHit.point - (Vector2)transform.position, Vector2.down);
			if (collidePointToDownAngle >= 60 && rightHit.point.x > transform.position.x) {
				collidingRightAngle = collidePointToDownAngle;
				collidingRight = true;	
			} else {
				collidingRightAngle = 0;
				collidingRight = false;
			}
		}
		else {
			collidingRight = false;
		}

		if (circleToLeft) {
			Debug.DrawLine (transform.position, circleToLeft.point, Color.green);
			float collidePointToDownAngle = Vector2.Angle (circleToLeft.point - (Vector2)transform.position, Vector2.down);
			if (collidePointToDownAngle >= 60 && circleToLeft.point.x < transform.position.x) {
				collidingLeftAngle = collidePointToDownAngle;
				collidingLeft = true;	
			} else {
				collidingLeftAngle = 0;
				collidingLeft = false;
			}
		} else {
			collidingLeft = false;
		}

		if ((collidingLeft || collidingRight) && !grounded) {
			climbing = true;
		} else {
			climbing = false;
		}

		if ((collidingLeft || collidingRight) && grounded) {
			onCorner = true;
		} else {
			onCorner = false;
		}
	}





	void Jump()
	{

		jumpTrigger = false;
		jumpCount += 1;

		if (grounded) {
			if (onCorner) {
				SetKinematic (false);
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
				rb2d.AddForce(new Vector2(0f, jumpForce));

				//Flip
				facingDirection = - facingDirection;
			} else {
				rb2d.AddForce(new Vector2(0f, jumpForce));
				rb2d.velocity = new Vector2 (rb2d.velocity.x, rb2d.velocity.y);
					
			}


		} else {
			
			rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
			rb2d.AddForce(new Vector2(0f, jumpForce));

			//Flip
			facingDirection = - facingDirection;
		}
	}

	void MoveControl()
	{


		float horizontalSpeed = moveSpeed * facingDirection;
		float verticalSpeed = rb2d.velocity.y;


		if (onCorner) {

			if (!rb2d.isKinematic) {
				SetKinematic (true);
			}

			horizontalSpeed = 0;
			verticalSpeed = 0;
		}
		
		if (!climbing) {
			if (rb2d.gravityScale == 0) {
				rb2d.gravityScale = 1;
			}
			rb2d.velocity = new Vector2 (horizontalSpeed, verticalSpeed);
		} else {
			rb2d.gravityScale = 0;
			rb2d.velocity = new Vector2 (rb2d.velocity.x, - wallSlidingSpeed);
		}


	}

	void SetKinematic(bool value)
	{
		if (value) {
			if (Time.time - kinematicCheckingTime > 0.5f) {
				rb2d.isKinematic = true;
			}

		} else {

			rb2d.isKinematic = false;
		}
		kinematicCheckingTime = Time.time;	
	}

}