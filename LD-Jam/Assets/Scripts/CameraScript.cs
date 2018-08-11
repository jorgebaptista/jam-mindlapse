using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
