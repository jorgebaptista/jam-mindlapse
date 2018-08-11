using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 pos = transform.position;
		BoxCollider2D bc = GetComponent<BoxCollider2D>();		
		if (bc == null) bc = GetComponentInParent<BoxCollider2D>();
		if (bc != null) {
			pos.z = pos.y + bc.offset.y;
		} else {
			pos.z = pos.y;
		}
		transform.position = pos;
	}
}
