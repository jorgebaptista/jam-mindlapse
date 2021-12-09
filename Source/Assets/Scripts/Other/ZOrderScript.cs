using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderScript : MonoBehaviour {

	bool isDynamic = false;

	// Use this for initialization
	void Start () {
		if (GetComponent<Rigidbody2D>() != null) isDynamic = true;
		if (GetComponentInParent<Rigidbody2D>() != null) isDynamic = true;	
		if (GetComponentInChildren<Rigidbody2D>() != null) isDynamic = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null) {
			Vector3 pos = transform.position;
			pos.z = renderer.bounds.min.y;
			transform.position = pos;
		}
		if (!isDynamic) Destroy(this); //remove script if object has no Rigidbody2D

		// Vector3 pos = transform.position;
		// BoxCollider2D bc = GetComponent<BoxCollider2D>();		
		// if (bc == null) bc = GetComponentInParent<BoxCollider2D>();
		// if (bc != null) {
		// 	pos.z = pos.y + bc.offset.y;
		// } else {
		// 	pos.z = pos.y;
		// }
		// transform.position = pos;
	}
}
