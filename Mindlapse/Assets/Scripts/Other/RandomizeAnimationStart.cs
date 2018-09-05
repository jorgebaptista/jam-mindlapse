using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAnimationStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().Play("", -1, Mathf.Repeat(transform.position.x, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
