using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [Header("Player Perception")]
    [Space]
    [SerializeField]
    private float perceptionRadius = 6f;

    [Header("References")]
    [Space]
    [SerializeField]
    private Transform target;

    [Space]
    [SerializeField]
    private float clayOffset = -1;
    [SerializeField]
    private GameObject clayPrefab;

    private int currentLife;
    private float horizontalPosition;

    private bool isAlive;
    private bool isAttacking;
    private bool attacked;
    private bool canFlip;

    //private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private Rigidbody2D myRigidBody2D;
    private Collider2D myCollider2D;

    private GameObject[] waypoints;
    private Vector2 currentWaypoint;

    private void Awake()
    {
        isAlive = true;
        currentLife = life;

        //mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();

        FindObjectOfType<SoundFXPlayer>().PlayMonsterSpawnSound();
        FindObjectOfType<SoundFXPlayer>().ToggleMonsterHoverSound(true);
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        FindNextWaypoint();
    }

    private void OnEnable()
    {
        isAlive = true;
        myAnimator.SetBool("Is Alive", true);
        myRigidBody2D.isKinematic = true;
        myCollider2D.enabled = true;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            horizontalPosition = transform.position.x;

            if (Physics2D.OverlapCircleNonAlloc(transform.position, perceptionRadius, new Collider2D[1], playerLayerMask) == 0)
            {
                if ((Vector2)transform.position != currentWaypoint)
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, movementSpeed * Time.deltaTime);
                    canFlip = true;
                }
                else
                {
                    FindNextWaypoint();
                }
            }
            else
            {
                if (transform.position.x < target.position.x - offset || transform.position.x > target.position.x + offset)
                {
                    canFlip = true;

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                }
                else
                {
                    canFlip = false;

                    if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.position.x + offset, 0))
                        < Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.position.x - offset, 0)))
                    {
                        transform.position = Vector2.MoveTowards(transform.position,
                            new Vector2(target.position.x + offset, transform.position.y), movementSpeed * Time.deltaTime);

                        if (!isFacingLeft && transform.position.x > target.position.x)
                        {
                            Flip();
                        }
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position,
                            new Vector2(target.position.x - offset, transform.position.y), movementSpeed * Time.deltaTime);

                        if (isFacingLeft && transform.position.x > target.position.x)
                        {
                            Flip();
                        }
                    }
                }

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y), movementSpeed * Time.deltaTime);
            }
            if (canFlip)
            {
                if ((transform.position.x > horizontalPosition && isFacingLeft) || (transform.position.x < horizontalPosition && !isFacingLeft))
                {
                    Flip();
                }
            }

            if (!isAttacking)
            {
                if (Physics2D.OverlapCircleNonAlloc(playerCheck.position, checkRadius, new Collider2D[1], playerLayerMask) > 0)
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

    private void FindNextWaypoint()
    {
        currentWaypoint = waypoints[Random.Range(0, waypoints.Length)].GetComponent<Transform>().position;
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
        FindObjectOfType<SoundFXPlayer>().ToggleMonsterHoverSound(false);
        isAlive = false;
        myCollider2D.enabled = false;
        myAnimator.SetTrigger("Die");
        myAnimator.SetBool("Is Alive", false);

        GameObject clay = Instantiate(clayPrefab);
        clay.transform.position = new Vector3(transform.position.x, transform.position.y + clayOffset);
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

        GameManagerScript.instance.UpdateNumberOfEnemies();
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

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Wall"))
    //    {
    //        mySpriteRenderer.color = new Color(1, 1, 1, 1);
    //    }
    //}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.forward, perceptionRadius);
    }
#endif
}
