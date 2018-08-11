using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAnimationScript : MonoBehaviour {

	public Transform throwingArm;
	public GameObject orb;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GameObject.Instantiate(orb, throwingArm.transform.TransformPoint(-1, 1, 0), Quaternion.identity);
		}
		if (Input.GetKey(KeyCode.Space)) {
			throwingArm.localRotation = Quaternion.Euler(0, 0, 0);
		} else {
			throwingArm.localRotation = Quaternion.Euler(0, 0, -90);
		}
	}
}
