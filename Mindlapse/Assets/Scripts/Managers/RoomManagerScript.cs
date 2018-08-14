using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerScript : MonoBehaviour
{
    [Header("Room Settings")]
    [Space]
    [Tooltip("Put rooms you want to disappear here FIRST.")]
    [SerializeField]
    private GameObject[] rooms;
    [Tooltip("Put rooms you want to disappear here LAST, example: the ones in the middle.")]
    [SerializeField]
    private GameObject[] lastRooms;

    [Header("Flash Settings")]
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

    public static RoomManagerScript instance;

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

        ResetFlash();
    }

    private void Start()
    {
        int totalRooms = GameObject.FindGameObjectsWithTag("Room").Length;

        int pickedRooms = rooms.Length + lastRooms.Length;

        if (totalRooms != pickedRooms)
        {
            Debug.LogError("Not all rooms were picked. Please drag and drop every room into the RoomManagerScript arrays in the 'Rooms' gameobject specifically.");
        }
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
        int activeLastRooms = 0;
        int totalActiveRooms = 0;

        for (int i = 0; i < rooms.Length; ++i)
        {
            if (!rooms[i].GetComponent<RoomScript>().isBroken)
            {
                ++activeRooms;
            }
        }
        for (int i = 0; i < lastRooms.Length; ++i)
        {
            if (!lastRooms[i].GetComponent<RoomScript>().isBroken)
            {
                ++activeLastRooms;
            }
        }

        totalActiveRooms = activeLastRooms + activeRooms;
        // choose last rooms to disappear
        if (totalActiveRooms > 0)
        {
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
            }
            else if (activeLastRooms > 0)
            {
                while (roomScript == null)
                {
                    RoomScript avaiableRoom = lastRooms[Random.Range(0, lastRooms.Length)].GetComponent<RoomScript>();

                    if (!avaiableRoom.isBroken)
                    {
                        roomScript = avaiableRoom;
                    }
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

    public void WallUpdate()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < walls.Length; ++i)
        {
            walls[i].GetComponent<WallScript>().ScanRoom();
        }
    }
}
