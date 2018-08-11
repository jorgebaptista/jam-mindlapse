using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Settings")]
    [Space]
    [SerializeField]
    private float movementSpeed = 3f;

    [Header("UI")]
    [Space]
    [SerializeField]
    private Image timerBar;
    //timerfillbar

    private float horizontalMove;
    private float verticalMove;

    private bool inFlashingRoom;
    private bool inBrokenRoom;

    private Rigidbody2D myRigidBody2D;
    private RoomScript roomScript;

    private void Awake()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Interact"))
        {
            if (inFlashingRoom)
            {
                StopFlash();
            }
            else if(inBrokenRoom)
            {
                RepairRoom();
            }
        }
        else
        {
            timerBar.fillAmount = 0;
        }
    }

    private void StopFlash()
    {
        if (GameManagerScript.instance.currentCoins >= GameManagerScript.instance.saveRoomPrice)
        {
            if (roomScript != null)
            {
                ToggleTimerBar(true);

                if (timerBar.fillAmount != 1)
                {
                    timerBar.fillAmount = Mathf.MoveTowards(timerBar.fillAmount, 1, GameManagerScript.instance.saveRoomSpeed * Time.deltaTime);
                }
                else
                {
                    roomScript.EndFlash();
                    GameManagerScript.instance.UpdateCoins(-GameManagerScript.instance.saveRoomPrice);
                    ToggleTimerBar(false);
                }
            }
            else
            {
                Debug.LogError("RoomScript not found.");
            }
        }
    }

    private void RepairRoom()
    {
        if (GameManagerScript.instance.currentCoins >= GameManagerScript.instance.repairRoomPrice)
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
                    roomScript.Repair();
                    GameManagerScript.instance.UpdateCoins(-GameManagerScript.instance.repairRoomPrice);
                    ToggleTimerBar(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        myRigidBody2D.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
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

    private void ToggleTimerBar(bool toggle)
    {
        timerBar.gameObject.SetActive(toggle);

        if (!toggle)
        {
            timerBar.fillAmount = 0;
        }
    }
}
