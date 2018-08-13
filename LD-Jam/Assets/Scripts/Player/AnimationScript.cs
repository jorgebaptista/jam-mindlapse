using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {

	int currentAnimationState = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (
			!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) &&
			!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) &&
			!(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&
			!(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		) SetAnimationState(0);

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) SetAnimationState(1);
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) SetAnimationState(2);
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) SetAnimationState(3);
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) SetAnimationState(4);
	}

	void SetAnimationState(int state) {
		if (state != currentAnimationState) {
			currentAnimationState = state;
			Animator animator = GetComponent<Animator>();

			if (state == 1) animator.SetTrigger("Up");
			if (state == 4) animator.SetTrigger("Down");
			if (state == 2 || state == 3) animator.SetTrigger("Right");

			if (state == 0) {
				animator.SetBool("Idle", true);
				return;
			} else {
				animator.SetBool("Idle", false);
			}

			if (state == 2)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180, transform.localEulerAngles.z);
            }
            else
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            }

		}
	}
}
