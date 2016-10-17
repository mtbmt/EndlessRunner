using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	[HideInInspector] public GameObject mainChar;
	[HideInInspector] public GameObject mainCamera;
	/*
	private Vector2 currentPosition;
	private int mcFacingDirection;
	public bool lockY;
	public float smoothTime;
	private float currentVelocity;
	public float reactTime;
	public Vector2 offSet;
	private Vector2 target;

	void Awake () {
		mainChar = GameObject.Find ("MC");
		mainCamera = gameObject;

	}


	void FixedUpdate () {
		mcFacingDirection = mainChar.GetComponent<CharController> ().facingDirection;
		Vector2 offSetUpdated = new Vector2 (offSet.x * mcFacingDirection, offSet.y);
		if (lockY) {
			target = Vector2.MoveTowards (target, (Vector2)mainChar.transform.position + offSetUpdated, offSet.x * Time.fixedDeltaTime / smoothTime);
			currentVelocity = Mathf.Abs(target.x - currentPosition.x) * reactTime;
			currentPosition = currentPosition + new Vector2((target.x - currentPosition.x)*currentVelocity * Time.fixedDeltaTime, 0);
		} else {
			target = Vector2.MoveTowards (target, (Vector2)mainChar.transform.position + offSetUpdated, Mathf.Sqrt(Vector2.SqrMagnitude(offSet)) * Time.fixedDeltaTime / smoothTime);
			currentVelocity = Vector2.Distance(target, currentPosition) * reactTime;
			currentPosition = currentPosition + (target - currentPosition)/Vector2.Distance(target, currentPosition) * currentVelocity * Time.fixedDeltaTime;
		}
		transform.position = new Vector3 (currentPosition.x, currentPosition.y, transform.position.z);
		Debug.DrawLine (target, (Vector2)mainChar.transform.position + offSetUpdated, Color.red);
		Debug.DrawLine (transform.position, target, Color.blue);

	}
	*/


	private int mcFacingDirection;
	public float acceleratorSpeed;
	private float velocity;
	private Vector2 target;
	private Vector2 leftTarget;
	private Vector2 rightTarget;
	public float targetSideDistance;
	public GameObject m_target;
	public GameObject m_leftTarget;
	public GameObject m_rightTarget;

	public Vector2 offSet;
	void Awake () {
		mainChar = GameObject.Find ("MC");
		mainCamera = gameObject;


	}

	void FixedUpdate () {

		CameraUpdate ();

		UpdateDebug ();

	}


	void CameraUpdate()
	{
		Vector3 offset = new Vector3 (0, 0, transform.position.z - mainChar.transform.position.z);
		transform.position = Vector3.Lerp (transform.position, mainChar.transform.position + offset, 0.1f);
	}

	void UpdateDebug()
	{
		m_target.transform.position = (Vector3)target;
		m_leftTarget.transform.position = (Vector3)leftTarget;
		m_rightTarget.transform.position = (Vector3)rightTarget;
	}


}

