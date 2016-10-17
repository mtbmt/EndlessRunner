using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public GameObject target;
	public float forceMax;
	public float brakingForceMax;
	public float brakingRange;
	public float mcMaxSpeed;
	public float cameraMaxSpeed;
	public LayerMask groundMask;
	private float currentVelocity;

	void Awake()
	{

	}
	void Update()
	{
		RaycastHit2D[] allCircleCast = Physics2D.CircleCastAll (transform.position, 1 * 1.1f, Vector3.zero, 0, groundMask);
		foreach (var hit in allCircleCast) {
			Vector2 rayDirection = (Vector2)hit.point - (Vector2)transform.position;
			RaycastHit2D rayCast = Physics2D.Raycast (transform.position, rayDirection, 1 * 1.1f, groundMask);
			//Draw all contact points
			Debug.DrawLine (transform.position, rayCast.point, Color.red);
			float rayAngle = Vector2.Angle (rayDirection, Vector2.down);


		}

	}

}
