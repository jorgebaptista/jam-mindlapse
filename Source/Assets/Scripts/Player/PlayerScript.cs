﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Space]
    [SerializeField]
    private bool canBeImmune = true;
    [SerializeField]
    private float immuneTimer = 2f;

    [Space]
    [SerializeField]
    private bool showRadiusAlways = true;

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
    private GameObject dummyOrb;
    [SerializeField]
    private GameObject radiusIndicator;

    [Space]
    [SerializeField]
    private GameObject orbPrefab;

    private int poolIndex;

    [Header("UI References")]
    [Space]
    [SerializeField]
    private Image timerBar;
    [SerializeField]
    private GameObject timerBarBack;

    [Space]
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private RectTransform clayIndicatorPos;
    [SerializeField]
    private GameObject clayIndicatorPrefab;

    [Space]
    [SerializeField]
    private GameObject dialogueOverlay;
    [SerializeField]
    private XMLReader dialogueSource;

    public static int currentClayPoints;

    private float horizontalMove;
    private float verticalMove;
    private float baseTimer;

    private bool inFlashingRoom;
    private bool inBrokenRoom;
    private bool isInteracting;
    private bool isImmune;

    private Animator myAnimator;
    private Rigidbody2D myRigidBody2D;
    private RoomScript roomScript;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();

        currentClayPoints = startingClayPoints;
    }

    private void Start()
    {
        UIManagerScript.instance.UpdateClayPointsText(currentClayPoints);

        poolIndex = PoolManagerScript.instance.PreCache(orbPrefab, 2);
    }

    bool goingUp = false;
    private void Update()
    {
        if (!GameManagerScript.instance.isPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove = Input.GetAxisRaw("Vertical");

            //Vector2 mousePos = Input.mousePosition;

            //if (mousePos.x > Screen.width / 2)
            //{
            //    if (mousePos.x > Screen.width / 3 && mousePos.x < Screen.width * 2 / 3 )
            //    {
            //        if (mousePos.y > (2 * Screen.height) / 3)
            //        {
            //            myAnimator.SetBool("Up", true);
            //            myAnimator.SetBool("Horizontal", false);
            //            myAnimator.SetBool("Down", false);
            //        }
            //        else if (mousePos.y < Screen.height / 3)
            //        {
            //            myAnimator.SetBool("Up", false);
            //            myAnimator.SetBool("Horizontal", false);
            //            myAnimator.SetBool("Down", true);
            //        }
            //    }
            //    else
            //    {
            //        myAnimator.SetBool("Up", false);
            //        myAnimator.SetBool("Horizontal", true);
            //        myAnimator.SetBool("Down", false);
            //    }
            //}
            //else
            //{
            //    if (mousePos.y < (2 * Screen.height) / 3)
            //    {
            //        myAnimator.SetBool("Up", true);
            //        myAnimator.SetBool("Horizontal", false);
            //        myAnimator.SetBool("Down", false);
            //    }
            //    else if (mousePos.y > Screen.height / 3)
            //    {
            //        myAnimator.SetBool("Up", false);
            //        myAnimator.SetBool("Horizontal", false);
            //        myAnimator.SetBool("Down", true);
            //    }
            //    else
            //    {
            //        myAnimator.SetBool("Up", false);
            //        myAnimator.SetBool("Horizontal", true);
            //        myAnimator.SetBool("Down", false);
            //    }
            //}
            if (!isInteracting)
            {
                if (showRadiusAlways)
                {
                    radiusIndicator.SetActive(true);
                    Vector2 radiusMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                    radiusIndicator.transform.position = new Vector2(radiusMousePos.x, radiusMousePos.y);
                }
                if (Time.time > baseTimer)
                {
                    if (!showRadiusAlways)
                    {
                        radiusIndicator.SetActive(true);
                        Vector2 radiusMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                        radiusIndicator.transform.position = new Vector2(radiusMousePos.x, radiusMousePos.y);
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        Shoot();
                    }
                }
                else if (!showRadiusAlways)
                {
                    radiusIndicator.SetActive(false);
                }

                if (verticalMove > 0) goingUp = true;
                if (verticalMove < 0 || horizontalMove != 0) goingUp = false;


                if (Input.GetButton("Fire1") && Time.time > baseTimer)
                {
                    if (goingUp)
                    {
                        arm.localPosition = new Vector3(-0.5f, 0.38f, 10f);
                        arm.localRotation = Quaternion.Euler(0, 0, -185.4f);
                        arm.localScale = new Vector3(1, -1, 1);
                    }
                    else
                    {
                        arm.localScale = new Vector3(1, 1, 1);
                        arm.localRotation = Quaternion.Euler(0, 0, -16f);
                        arm.localPosition = new Vector3(0.7f, 0.46f, 10f);
                    }

                    dummyOrb.SetActive(true);
                }
                else
                {
                    if (goingUp)
                    {
                        arm.localPosition = new Vector3(-0.45f, 0.38f, 10f);
                        arm.localRotation = Quaternion.Euler(0, 0, -70f);//110f);
                        arm.localScale = new Vector3(1, -1, 1);
                    }
                    else
                    {
                        arm.localScale = new Vector3(1, 1, 1);
                        arm.localRotation = Quaternion.Euler(0, 0, -110f);
                        arm.localPosition = new Vector3(0.41f, 0.53f, 10f);
                    }

                    dummyOrb.SetActive(false);
                }
            }

            if (Input.GetButton("Interact"))
            {
                isInteracting = true;

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
                isInteracting = false;
            }
        }
    }

    private void FixedUpdate()
    {
        myRigidBody2D.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);

        //myAnimator.SetFloat("Movement", Math.Abs(Math.Max(horizontalMove, verticalMove)));
    }

    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            UpdateClayPoints(-damage);

            FindObjectOfType<SoundFXPlayer>().PlayOuchSound();
        }
    }

    public void UpdateClayPoints(int value)
    {
        GameObject clayIndicator = Instantiate(clayIndicatorPrefab, canvas.transform);
        TextMeshProUGUI clayIndicatorText = clayIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        clayIndicatorText.text = value.ToString();
        clayIndicatorText.color = value < 0 ? Color.red : Color.green;

        clayIndicator.gameObject.SetActive(false);
        clayIndicator.gameObject.SetActive(true);

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
        FindObjectOfType<SoundFXPlayer>().PlaySanityGainSound();

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

        GameObject orb = PoolManagerScript.instance.GetCachedPrefab(poolIndex);

        if (orb != null)
        {
            orb.transform.position = shootPoint.position;
            orb.SetActive(true);
            orb.GetComponent<OrbScript>().ProjectTo(mousePos, shootSpeed, damage);

            arm.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            Debug.LogError("Orb in poolmanager not found.");
        }
    }

    private void RepairRoom()
    {
        if (currentClayPoints >= GameManagerScript.instance.saveRoomPrice)
        {
            if (roomScript != null)
            {
                string dialogue = dialogueSource.GetCharacterDialogue("Room_Repair", currentClayPoints);
                dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);

                FindObjectOfType<SoundFXPlayer>().ToggleRoomRepairSound(true);

                ToggleTimerBar(true);

                if (timerBar.fillAmount != 1)
                {
                    timerBar.fillAmount = Mathf.MoveTowards(timerBar.fillAmount, 1, GameManagerScript.instance.saveRoomSpeed * Time.deltaTime);
                }
                else
                {
                    roomScript.EndFlash();
                    UpdateClayPoints(-GameManagerScript.instance.saveRoomPrice);
                    FindObjectOfType<SoundFXPlayer>().ToggleRoomRepairSound(false);
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

                FindObjectOfType<SoundFXPlayer>().ToggleRoomRebuildSound(true);

                if (timerBar.fillAmount != 1)
                {
                    timerBar.fillAmount = Mathf.MoveTowards(timerBar.fillAmount, 1, GameManagerScript.instance.repairRoomSpeed * Time.deltaTime);
                }
                else
                {
                    roomScript.Rebuild();
                    UpdateClayPoints(-GameManagerScript.instance.repairRoomPrice);
                    FindObjectOfType<SoundFXPlayer>().ToggleRoomRebuildSound(false);
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

    public void RoomWarning()
    {
        string dialogue = dialogueSource.GetCharacterDialogue("Room_Warning", currentClayPoints);
        dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);
    }

    public void RoomBuild()
    {
        string dialogue = dialogueSource.GetCharacterDialogue("Room_Build", currentClayPoints);
        dialogueOverlay.GetComponent<DialogueContainerScript>().Display(dialogue);
    }

    public void MakeImmune()
    {
        if (!isImmune && canBeImmune)
        {
            isImmune = true;

            CancelInvoke("Immunity");
            Invoke("Immunity", immuneTimer);
        }
    }
    private void Immunity()
    {
        isImmune = false;
    }

    private void ToggleTimerBar(bool toggle)
    {
        timerBar.gameObject.SetActive(toggle);
        timerBarBack.SetActive(toggle);

        if (!toggle)
        {
            timerBar.fillAmount = 0;
        }
    }
}
