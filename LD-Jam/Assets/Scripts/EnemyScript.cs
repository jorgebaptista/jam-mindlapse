using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float movementSpeed = 3f;

    [Space]
    [SerializeField]
    private Transform target;
    

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
    }
}
