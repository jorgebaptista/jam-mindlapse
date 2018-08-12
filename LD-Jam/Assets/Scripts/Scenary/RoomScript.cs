using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private float flashDuration;
    private float fastFlashTimer;

    private float flashSpeed;
    private float fastFlashSpeed;

    private Color flashColor;
    private Color fastFlashColor;

    private bool isFlashing;
    [HideInInspector]
    public bool isBroken;
    private bool hasPlayerIn;

    private BoxCollider2D myBoxCollider2D;
    private SpriteRenderer[] myChildSpriteRenderers;

    private RoomManagerScript roomManagerScript;
    private PlayerScript playerScript;

    private void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        myChildSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        roomManagerScript = FindObjectOfType<RoomManagerScript>();
    }

    private void Update()
    {
        if (isFlashing)
        {
            Flash();
        }
    }

    public void StartFlash(float duration, float fastTimer, float normalSpeed, float fastSpeed, Color normalColor, Color fastColor)
    {
        isFlashing = true;
        flashDuration = duration + Time.time;
        fastFlashTimer = fastTimer + Time.time;
        flashSpeed = normalSpeed;
        flashColor = normalColor;
        fastFlashColor = fastColor;
    }

    private void Flash() // rename to crack when we have animations for it
    {
        if (flashDuration > Time.time)
        {
            if (fastFlashTimer > Time.time)
            {
                for (int i = 0; i < myChildSpriteRenderers.Length; ++i)
                {
                    myChildSpriteRenderers[i].color = Color.Lerp(Color.white, flashColor, Mathf.PingPong(Time.time, flashSpeed));
                }
            }
            else
            {
                for (int i = 0; i < myChildSpriteRenderers.Length; ++i)
                {
                    //doesn't work?!?!?!?! 
                    //myChildSpriteRenderers[i].color = Color.Lerp(Color.white, fastFlashColor, Mathf.PingPong(Time.time, fastFlashSpeed));
                    myChildSpriteRenderers[i].color = fastFlashColor;
                }
            }
        }
        else
        {
            Break();
        }
    }

    public void EndFlash()
    {
        isFlashing = false;

        for (int i = 0; i < myChildSpriteRenderers.Length; ++i)
        {
            myChildSpriteRenderers[i].color = Color.white;
        }

        roomManagerScript.ResetFlash();
    }

    private void Break()
    {
        EndFlash();
        roomManagerScript.ResetFlash();
        isBroken = true;

        myBoxCollider2D.size = new Vector2(myBoxCollider2D.size.x * 1.9f, myBoxCollider2D.size.y * 1.9f);

        for (int i = 0; i < myChildSpriteRenderers.Length; ++i)
        {
            myChildSpriteRenderers[i].color = Color.white;
        }

        InvertActiveChildren();

        if (hasPlayerIn)
        {
            GameManagerScript.instance.GameOver();
        }
        else
        {
            // This doesn't work at the moment. We need to get dialogue to show from here.
            // playerScript = playerScript ?? other.GetComponent<PlayerScript>();
            // playerScript.RoomBroken();
        }
    }

    public void Rebuild()
    {
        isBroken = false;

        myBoxCollider2D.size = new Vector2(myBoxCollider2D.size.x / 1.9f, myBoxCollider2D.size.y / 1.9f);

        InvertActiveChildren();
    }

    private void InvertActiveChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(!child.activeInHierarchy);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayerIn = true;

            playerScript  = playerScript ?? other.GetComponent<PlayerScript>();

            if (!isBroken)
            {
                if (isFlashing)
                {
                    playerScript.ToggleInFlashingRoom(true, this);
                }
                else
                {
                    playerScript.ToggleInFlashingRoom(false);
                }
            }
            else if (isBroken)
            {
                playerScript.ToggleInBrokenRoom(true, this);
            }
            else
            {
                //playerScript.ToggleInFlashingRoom(false);
                playerScript.ToggleInBrokenRoom(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayerIn = false;

            if (playerScript == null)
            {
                playerScript = other.GetComponent<PlayerScript>();
            }

            if (!isBroken)
            {
                playerScript.ToggleInFlashingRoom(false);
            }
            else if (isBroken)
            {
                playerScript.ToggleInBrokenRoom(false);
            }
        }
    }
}
