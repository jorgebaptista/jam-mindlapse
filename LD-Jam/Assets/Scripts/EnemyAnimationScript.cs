using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) animator.SetTrigger("Idle");
		if (Input.GetKeyDown(KeyCode.Alpha2)) animator.SetTrigger("Attack");
		if (Input.GetKeyDown(KeyCode.Alpha3)) animator.SetTrigger("Death");
	}
}
