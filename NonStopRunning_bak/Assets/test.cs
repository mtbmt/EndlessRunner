using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	float lerpTime = 1f;
	float currentLerpTime;

	float moveDistance = 10f;

	Vector3 startPos;
	Vector3 endPos;

	protected void Start() {
		startPos = transform.position;
		endPos = transform.position + transform.up * moveDistance;
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

	}
}
