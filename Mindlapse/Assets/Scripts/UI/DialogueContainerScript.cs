using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System;

public class DialogueContainerScript : MonoBehaviour
{
    [SerializeField]
    private long textDisplayTime;
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private Stopwatch timer = new Stopwatch();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.gameObject.activeInHierarchy && timer.Elapsed >= TimeSpan.FromSeconds(textDisplayTime))
        {
            this.gameObject.SetActive(false);
            timer.Reset();
        }
    }

    public void Display(string dialogue, bool replace = false)
    {
        if (String.IsNullOrEmpty(dialogue)) return;
        if (this.gameObject.activeInHierarchy) return;

        dialogueText.text = dialogue;
        this.gameObject.SetActive(true);

        timer.Start();
    }
}
