using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDamageable
{
    [Header("General")]
    [Space]
    [SerializeField]
    private int startingClayPoints = 0;
    [SerializeField]
    private int maxClayPoints = 30;

    [Space]
    [SerializeField]
    private float movementSpeed = 3f;

    [Header("Attack")]
    [Space]
    [SerializeField]
    private int damage = 2;
    [SerializeField]
    private float shootSpeed = 2f;
    [SerializeField]
    private float cooldown = 1f;

    [Header("Attack References")]
    [Space]
    [SerializeField]
    private Transform arm;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject orbPrefab;

    [Header("UI References")]
    [Space]
    [SerializeField]
    private Image timerBar;
    [SerializeField]
    private GameObject dialogueOverlay;
    [SerializeField]
    private XMLReader dialogueSource;
    //timerfillbar

    private int currentClayPoints;

    private float horizontalMove;
    private float verticalMove;
    private float baseTimer;

    private bool inFlashingRoom;
    private bool inBrokenRoom;

    private Rigidbody2D myRigidBody2D;
    private RoomScript roomScript;

    private void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();

        currentClayPoints = startingClayPoints;
    }

    private void Start()
    {
        UIManagerScript.instance.UpdateClayPointsText(currentClayPoints);
    }

    private void Update()
    {
        if (!GameManagerScript.instance.isPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove = Input.GetAxisRaw("Vertical");

            if (Time.time > baseTimer)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }

            if (Input.GetButton("Fire1"))
            {
                arm.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                arm.localRotation = Quaternion.Euler(0, 0, -90);
            }

            if (Input.GetButton("Interact"))
            {
                if (inFlashingRoom)
                {
                    RepairRoom();
                }
                else if (inBrokenRoom)
                {
                    RebuildRoom();
                }
                else
                {
                    ToggleTimerBar(false);
                }
            }
            else
            {
                ToggleTimerBar(false);
            }
        }
    }

    private void FixedUpdate()
    {
        myRigidBody2D.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
    }

    public void TakeDamage(int damage)
    {
        UpdateClayPoints(-damage);
        // Play ouch sound
    }

    public void UpdateClayPoints(int value)
    {
        currentClayPoints += value;
        bool increased = value > 0;

        if (currentClayPoints < 0)
        {
            currentClayPoints = 0;
        }
        else if (currentClayPoints > maxClayPoints)
        {
            currentClayPoints = maxClayPoints;
        }

        UpdateClayPointsUI();

        if (increased)
        {
            SanityGained();
        }
    }

    private void SanityGained()
    {
        // Play sanity gained sound

        int testRemainder;
        Math.DivRem(currentClayPoints, 5, out testRemainder);
        if (testRemainder == 0)
        {
            string dialogue = dialogueSource.GetCharacterDialogue("Sanity_Text", currentClayPoints);
            dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);
        }
    }

    private void UpdateClayPointsUI()
    {
        UIManagerScript.instance.UpdateClayPointsText(currentClayPoints);
    }

    private void Shoot()
    {
        arm.localRotation = Quaternion.Euler(0, 0, 0);

        baseTimer = Time.time + cooldown;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

        FindObjectOfType<SoundFXPlayer>().PlaySpellCastSound();
        string dialogue = dialogueSource.GetCharacterDialogue("Spell_Cast", currentClayPoints);
        dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);

        GameObject orb = Instantiate(orbPrefab);
        orb.transform.position = shootPoint.position;
        orb.GetComponent<OrbScript>().ProjectTo(mousePos, shootSpeed);

        arm.localRotation = Quaternion.Euler(0, 0, -90);
    }

    private void RepairRoom()
    {
        // Loop repairing sound
        if (currentClayPoints >= GameManagerScript.instance.saveRoomPrice)
        {
            if (roomScript != null)
            {
                string dialogue = dialogueSource.GetCharacterDialogue("Room_Repair", currentClayPoints);
                dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);

                ToggleTimerBar(true);

                if (timerBar.fillAmount != 1)
                {
                    timerBar.fillAmount = Mathf.MoveTowards(timerBar.fillAmount, 1, GameManagerScript.instance.saveRoomSpeed * Time.deltaTime);
                }
                else
                {
                    roomScript.EndFlash();
                    UpdateClayPoints(-GameManagerScript.instance.saveRoomPrice);
                    ToggleTimerBar(false);
                }
            }
            else
            {
                Debug.LogError("RoomScript not found.");
            }
        }
    }

    private void RebuildRoom()
    {
        if (currentClayPoints >= GameManagerScript.instance.repairRoomPrice)
        {
            if (roomScript != null)
            {
                ToggleTimerBar(true);

                if (timerBar.fillAmount != 1)
                {
                    timerBar.fillAmount = Mathf.MoveTowards(timerBar.fillAmount, 1, GameManagerScript.instance.repairRoomSpeed * Time.deltaTime);
                }
                else
                {
                    roomScript.Rebuild();
                    UpdateClayPoints(-GameManagerScript.instance.repairRoomPrice);
                    ToggleTimerBar(false);

                    string dialogue = dialogueSource.GetCharacterDialogue("Room_Rebuilt", currentClayPoints);
                    dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);
                }
            }
        }
    }

    public void ToggleInFlashingRoom(bool toggle, RoomScript script = null)
    {
        inFlashingRoom = toggle;
        roomScript = script;
    }

    public void ToggleInBrokenRoom(bool toggle, RoomScript script = null)
    {
        inBrokenRoom = toggle;
        roomScript = script;
    }

    public void RoomBroken()
    {
        string dialogue = dialogueSource.GetCharacterDialogue("Room_Destroyed", currentClayPoints);
        dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);
    }

    private void ToggleTimerBar(bool toggle)
    {
        timerBar.gameObject.SetActive(toggle);

        if (!toggle)
        {
            timerBar.fillAmount = 0;
        }
    }
}
