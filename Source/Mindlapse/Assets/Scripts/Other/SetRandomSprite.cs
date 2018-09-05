using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetRandomSprite : MonoBehaviour {

	public Sprite[] sprites;

	// Use this for initialization
	void Start () {
		int value = Mathf.FloorToInt(Mathf.Abs(transform.position.x + transform.position.y));
		GetComponent<SpriteRenderer>().sprite = sprites[(int)Mathf.Repeat(value, sprites.Length)];	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
