using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private RoomScript roomToScan;

    private SpriteRenderer mySpriteRenderer;
    private Collider2D myCollider2D;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ScanRoom();
    }

    public void ScanRoom()
    {
        if (!roomToScan.isBroken)
        {
            mySpriteRenderer.enabled = false;
            myCollider2D.enabled = false;
        }
        else
        {
            mySpriteRenderer.enabled = true;
            myCollider2D.enabled = true;
        }
    }
}
