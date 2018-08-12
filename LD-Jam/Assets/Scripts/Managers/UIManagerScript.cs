using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    [Header("Settings")]
    [Space]
    [SerializeField]
    private Text coinText;
    [SerializeField]
    private GameObject gameOverScreen;

    public static UIManagerScript instance;

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

    public void UpdateClayPointsText(int value)
    {
        coinText.text = value.ToString();
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}
