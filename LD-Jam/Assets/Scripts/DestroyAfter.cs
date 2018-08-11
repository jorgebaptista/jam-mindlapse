using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

	public int delay = 2;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
