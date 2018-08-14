using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomScript : MonoBehaviour
{
    [SerializeField]
    private GameObject repairCanvas;
    [SerializeField]
    private GameObject rebuildCanvas;

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
    
    private PlayerScript playerScript;

    private PlayerScript PlayerScriptHolder
    {
        get
        {
            playerScript = playerScript ?? GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            return playerScript;
        }
    }

    private void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        myChildSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFlashing)
        {
            Flash();

            repairCanvas.SetActive(true);

            TextMeshProUGUI repairTextMesh = repairCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (PlayerScript.currentClayPoints < GameManagerScript.instance.saveRoomPrice)
            {
                repairTextMesh.color = Color.red;
            }
            else
            {
                repairTextMesh.color = Color.yellow;
            }
        }
        else
        {
            repairCanvas.SetActive(false);
        }

        if (isBroken && hasPlayerIn)
        {
            rebuildCanvas.SetActive(true);

            TextMeshProUGUI rebuildTextMesh = rebuildCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (PlayerScript.currentClayPoints < GameManagerScript.instance.repairRoomPrice)
            {
                rebuildTextMesh.color = Color.red;
            }
            else
            {
                rebuildTextMesh.color = Color.yellow;
            }
        }
        else
        {
            rebuildCanvas.SetActive(false);
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

        //PlayerScriptHolder.RoomWarning();
        FindObjectOfType<SoundFXPlayer>().PlayRoomCrackSound();
    }

    private void Flash()
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

        RoomManagerScript.instance.ResetFlash();
    }

    private void Break()
    {
        EndFlash();
        RoomManagerScript.instance.ResetFlash();
        isBroken = true;

        myBoxCollider2D.size = new Vector2(myBoxCollider2D.size.x * 1.9f, myBoxCollider2D.size.y * 1.9f);

        for (int i = 0; i < myChildSpriteRenderers.Length; ++i)
        {
            myChildSpriteRenderers[i].color = Color.white;
        }

        DestroyChildren();

        if (hasPlayerIn)
        {
            FindObjectOfType<SoundFXPlayer>().PlayFallSound();
            GameManagerScript.instance.GameOver();
        }
        else
        {
            //PlayerScriptHolder.RoomBroken();
            FindObjectOfType<SoundFXPlayer>().PlayRoomCollapseSound();
        }

        RoomManagerScript.instance.WallUpdate();

    }

    public void Rebuild()
    {
        isBroken = false;

        myBoxCollider2D.size = new Vector2(myBoxCollider2D.size.x / 1.9f, myBoxCollider2D.size.y / 1.9f);

        RebuildChildren();

        RoomManagerScript.instance.WallUpdate();
    }

    private void DestroyChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            Animator animator = child.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Collapse");
            }
            else
            {
                child.SetActive(!child.activeInHierarchy);
            }
        }

        repairCanvas.SetActive(false);
        //rebuildCanvas.SetActive(true);
    }
    private void RebuildChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (!child.CompareTag("UI"))
            {
                child.SetActive(!child.activeInHierarchy);

                Animator animator = child.GetComponent<Animator>();

                if (animator != null)
                {
                    animator.SetTrigger("Rebuild");
                }
            }
        }

        //rebuildCanvas.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayerIn = true;

            PlayerScript playerScriptTemp  = other.GetComponentInParent<PlayerScript>();

            if (!isBroken)
            {
                if (isFlashing)
                {
                    playerScriptTemp.ToggleInFlashingRoom(true, this);
                }
                else
                {
                    playerScriptTemp.ToggleInFlashingRoom(false);
                }
            }
            else if (isBroken)
            {
                playerScriptTemp.ToggleInBrokenRoom(true, this);
                playerScriptTemp.RoomBuild();
            }
            else
            {
                //PlayerScriptHolder.ToggleInFlashingRoom(false);
                playerScriptTemp.ToggleInBrokenRoom(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayerIn = false;

            PlayerScript playerScriptTemp = other.GetComponentInParent<PlayerScript>();

            if (!isBroken)
            {
                playerScriptTemp.ToggleInFlashingRoom(false);
            }
            else if (isBroken)
            {
                playerScriptTemp.ToggleInBrokenRoom(false);
            }
        }
    }
}
