using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayScript : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 5f;

    [Space]
    [SerializeField]
    private int clayPoints = 1;

    private Rigidbody2D myRigidbody2D;

    private float initialPosY;

    private bool isJumping;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialPosY = transform.position.y;

        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            isJumping = false;

            myRigidbody2D.velocity = Vector2.up * jumpForce;
        }
        else if (transform.position.y < initialPosY)
        {
            myRigidbody2D.isKinematic = true;
            myRigidbody2D.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerScript>().UpdateClayPoints(clayPoints);
            Destroy(gameObject);
        }
    }
}
