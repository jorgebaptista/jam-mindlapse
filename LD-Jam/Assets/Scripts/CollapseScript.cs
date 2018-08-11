using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseScript : MonoBehaviour {


	bool isCollapsed = false;

	public void Collapse() {
		if (!isCollapsed) {
			Animator animator = GetComponent<Animator>();
			animator.SetTrigger("Collapse");
			isCollapsed = true;
			//Destroy(gameObject, 0.8f);
		}
	}

	void Update() {
		if (Input.GetKey(KeyCode.Y) && Random.Range(0, 15) == 0) Collapse();
	}

}
