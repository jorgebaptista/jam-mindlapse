using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerScript : MonoBehaviour
{
    [Header("Rooms Settings")]
    [Space]
    [SerializeField]
    [Tooltip("Time until random room starts flashing.")]
    private float roomTimer = 10f;

    [Space]
    [SerializeField]
    [Tooltip("Total duration of the flash.")]
    private float flashDuration = 7f;
    [SerializeField]
    [Tooltip("Time until it starts flashing faster.")]
    private float fastFlashTimer = 5f;

    [Header("Normal Flash Settings")]
    [Space]
    [SerializeField]
    private float flashSpeed = 0.5f;
    [SerializeField]
    private Color flashColor = Color.yellow;

    [Header("Fast Flash Settings")]
    [Space]
    [SerializeField]
    private float fastFlashSpeed = 0.2f;
    [SerializeField]
    private Color fastFlashColor = Color.red;

    private float baseTimer;

    private bool canFlash;

    private RoomScript roomScript;

    private GameObject[] rooms;

    private void Awake()
    {
        ResetFlash();
    }

    private void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");
    }

    private void Update()
    {
        if (canFlash && Time.time > baseTimer)
        {
            CallRandomRoom();
            baseTimer = 0;
            canFlash = false;
        }
    }

    private void CallRandomRoom()
    {
        int activeRooms = 0;

        for (int i = 0; i < rooms.Length; ++i)
        {
            if (!rooms[i].GetComponent<RoomScript>().isBroken)
            {
                ++activeRooms;
            }
        }

        if (activeRooms > 0)
        {
            while (roomScript == null)
            {
                RoomScript avaiableRoom = rooms[Random.Range(0, rooms.Length)].GetComponent<RoomScript>();

                if (!avaiableRoom.isBroken)
                {
                    roomScript = avaiableRoom;
                }
            }

            roomScript.StartFlash(flashDuration, fastFlashTimer, flashSpeed, fastFlashSpeed, flashColor, fastFlashColor);
        }
        else
        {
            GameManagerScript.instance.GameOver();
        }
        
    }

    public void ResetFlash()
    {
        canFlash = true;
        roomScript = null;
        baseTimer = roomTimer + Time.time;
    }
}
