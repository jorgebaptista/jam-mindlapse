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
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) SetAnimationState(1);
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) SetAnimationState(2);
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) SetAnimationState(3);
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) SetAnimationState(4);
		else SetAnimationState(0);
	}

	void SetAnimationState(int state) {
		if (state != currentAnimationState) {
			currentAnimationState = state;

			Animator animator = GetComponent<Animator>();

			if (state == 0) {
				animator.SetBool("Idle", true);
			} else {
				animator.SetBool("Idle", false);
			}

			if (state == 2 || state == 3) animator.SetTrigger("Right");

			if (state == 2) {
				transform.localScale = new Vector2(-1, 1);
			} else {
				transform.localScale = new Vector2(1, 1);
			}

			if (state == 1) animator.SetTrigger("Up");
			if (state == 4) animator.SetTrigger("Down");
		}
	}
}
