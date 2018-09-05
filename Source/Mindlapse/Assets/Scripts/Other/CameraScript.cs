using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Smooth Follow Settings")]
    [Space]
    [SerializeField]
    private float speed = 0.2f;

    [Header("Explosion Shake")]
    [Space]
    [SerializeField]
    private float shakeDuration = 0.5f;
    [SerializeField]
    private float shakeRadius = 2f;


    [Space]
    [SerializeField]
    private Transform target;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lastOffsetPosition = Vector3.zero;

    public static CameraScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), ref velocity, speed);
    }

    public void ShakeCamera()
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(shakeDuration, shakeRadius));
    }

    private IEnumerator DoShake(float duration, float radius)
    {
        while (duration > 0)
        {
            transform.localPosition -= lastOffsetPosition;

            lastOffsetPosition = Random.insideUnitCircle * radius;

            transform.localPosition += lastOffsetPosition;

            if (duration < 0.5f)
            {
                radius *= 0.9f;
            }

            duration -= Time.deltaTime;
            yield return null;
        }
    }
}
