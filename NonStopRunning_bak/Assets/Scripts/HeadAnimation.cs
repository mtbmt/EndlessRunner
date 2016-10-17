using UnityEngine;
using System.Collections;

public class HeadAnimation : MonoBehaviour {
	float angle;
	public  float maxRotation;
	// Use this for initialization
	void Start () {
		angle = 0;
	}
	
	// Update is called once per frame
	void Update () {
		angle = angle + Mathf.PI * Time.deltaTime;

		transform.rotation = new Quaternion (0, 0, Mathf.Sin (angle) / 90 * maxRotation, 1);
	}
}
