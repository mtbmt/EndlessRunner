using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public CharController charController;

	void Update () {
		if (Input.touchCount > 0) {
			print ("asdasd");
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
				charController.jumpTrigger = true;		

			}
		}
	}
}
