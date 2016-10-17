using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	
	public int facingDirection; // right = 1, left = -1
	public Rigidbody2D rb2d;
	public BoxCollider2D boxCollider;
	public LayerMask groundMask;
	private int numberOfRaycast;
	private RaycastHit2D[] bottomHits;
	private RaycastHit2D[] aheadHits;
	private float colliderSizeX;
	private float colliderSizeY;

	private RaycastHit2D bottomBoxHit;
	private RaycastHit2D aheadBoxHit;


	public enum CharacterState {Idle, Running, Jumping, Falling, Sliding};
	public CharacterState charState;

	public float grounded;


	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		numberOfRaycast = 4;
		bottomHits = new RaycastHit2D[numberOfRaycast];
		aheadHits = new RaycastHit2D[numberOfRaycast];
		colliderSizeX = boxCollider.size.x * transform.localScale.x;
		colliderSizeY = boxCollider.size.y * transform.localScale.y;


	}

	void FixedUpdate()
	{
		InputControl ();

		RayHitUpdate ();

		StateUpdate ();

		MoveControl ();
		print (rb2d.velocity);
		//rb2d.velocity = Vector2.up * Time.fixedDeltaTime * Physics2D.gravity.y * Time.fixedDeltaTime;
		rb2d.velocity = Vector2.zero;
	}




	void InputControl()
	{
		if (Mathf.Abs(Input.GetAxis("Horizontal")) != 0) {
			rb2d.AddForce (Vector2.right * Input.GetAxis ("Horizontal") * Time.fixedDeltaTime * 100f);
		}
	}


	void MoveControl()
	{
		//Handle whenever MC collide with ground
		float horizontalVelocity = rb2d.velocity.y;
		if (bottomBoxHit) {
			float minorGap = horizontalVelocity * Time.fixedDeltaTime - transform.position.y + bottomBoxHit.point.y + colliderSizeY / 2;
//			if (horizontalVelocity * Time.fixedDeltaTime - transform.position.y + bottomBoxHit.point.y + colliderSizeY / 2 > 0) {
			if (true) {
				print (rb2d.velocity.x);
				//rb2d.velocity = Vector2.zero;//new Vector2 (0, rb2d.velocity.y);
				print("sadsa");
			}	
		}

	}



	void StateUpdate()
	{
		// ground check
		for (int i = 0; i < bottomHits.Length; i++) {
			
			if (bottomHits[i]) {
				//print (string.Format("{0} {1}", hit, hit.distance));
				if (Mathf.Abs(bottomHits[i].distance - colliderSizeY/2) < 0.01f) {
					//print (string.Format("{0} {1}", i, bottomHits[i].distance));
				}
			}
		}
	
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
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)bottomHits[i].point, Color.red);
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
				//Debug.DrawLine ((Vector3)startPosition, (Vector3)aheadHits[i].point, Color.blue);
			}
		}






		//bottomBoxHit
		Vector2 boxSize = new Vector2(boxCollider.size.x * transform.localScale.x, boxCollider.size.y * transform.localScale.y);
		bottomBoxHit = Physics2D.BoxCast (transform.position, boxSize, 0, Vector2.down, 1f, groundMask);



	}

}