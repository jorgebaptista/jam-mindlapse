using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [Space]
    [SerializeField]
    private int life = 1;
    [SerializeField]
    private float movementSpeed = 3f;

    [Space]
    [SerializeField]
    private float dieFallingTime = 2f;

    [Space]
    [SerializeField]
    private bool isFacingLeft = true;

    [Header("Attack Settings")]
    [Space]
    [SerializeField]
    private int damage = 1;

    [Space]
    [SerializeField]
    private float offset = 2f;
    [SerializeField]
    private Collider2D attackTrigger;

    [Space]
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private float checkRadius = 0.2f;

    private Collider2D[] checkColliders = new Collider2D[1];

    [Header("References")]
    [Space]
    [SerializeField]
    private Transform target;
    private Animator myAnimator;
    private Rigidbody2D myRigidBody2D;

    private int currentLife;
    private float horizontalPosition;

    private bool isAlive;
    private bool isAttacking;
    private bool attacked;

    private void Awake()
    {
        isAlive = true;
        currentLife = life;
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void OnEnable()
    {
        myRigidBody2D.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            horizontalPosition = transform.position.x;

            if (transform.position.x < target.position.x - offset || transform.position.x > target.position.x + offset)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), movementSpeed * Time.deltaTime);
            }

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y), movementSpeed * Time.deltaTime);

            if ((transform.position.x > horizontalPosition && isFacingLeft) || (transform.position.x < horizontalPosition && !isFacingLeft))
            {
                Flip();
            }

            if (!isAttacking)
            {
                if (Physics2D.OverlapCircleNonAlloc(playerCheck.position, checkRadius, checkColliders, playerLayerMask) > 0)
                {
                    Attack();
                }
            }
        }
        else
        {
            myRigidBody2D.isKinematic = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isAlive)
        {
            currentLife -= damage;

            if (currentLife <= 0)
            {
                currentLife = 0;
                Die();
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        myAnimator.SetTrigger("Die");
    }

    public void Dismiss()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(dieFallingTime);

        gameObject.SetActive(false);
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
        isAttacking = true;
    }

    public void ToggleAttack(bool toggle)
    {
        attackTrigger.enabled = toggle;

        if (!toggle)
        {
            isAttacking = false;
            attacked = false;
        }
    }

    private void Flip()
    {
        Vector3 myLocalRotation = transform.localEulerAngles;
        myLocalRotation.y += 180f;
        transform.localEulerAngles = myLocalRotation;
        isFacingLeft = !isFacingLeft;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attacked)
        {
            if (other.GetComponent<PlayerScript>())
            {
                other.GetComponent<PlayerScript>().TakeDamage(damage);
                attacked = true;
            }
        }
    }
}
