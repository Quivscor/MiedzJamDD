﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class TutorialController : MonoBehaviour
{
    public List<TutorialSequence> sequences;
    private int currentSequenceIndex = 0;
    private int currentDialogueInSequenceIndex = 0;

    public GameObject TutorialHUD;
    public TMProText dialogueText;
    public Image tutorialLadyImage;

    private void Awake()
    {
        BuildingsInventory.Instance.OnEmptyInventory += DisplaySequence; //InventoryTutorial
    }

    private void Start()
    {
        //DisplaySequence(0);
    }

    public void DisplaySequence(int sequenceID)
    {
        if (sequences[currentSequenceIndex].hasFired)
            return;

        currentSequenceIndex = sequenceID;
        sequences[currentSequenceIndex].hasFired = true;

        TutorialHUD.SetActive(true);
        DisplayDialogue();
    }    

    public void DisplayDialogue()
    {
        dialogueText.text = sequences[currentSequenceIndex].dialogues[currentDialogueInSequenceIndex].dialogueMsg;
        tutorialLadyImage.sprite = sequences[currentSequenceIndex].dialogues[currentDialogueInSequenceIndex].dialogueSprite;
    }

    public void ButtonNext()
    {
        currentDialogueInSequenceIndex++;
        if (currentDialogueInSequenceIndex == sequences[currentSequenceIndex].dialogues.Count)
            HideDisplay();
        else
            DisplayDialogue();
    }

    public void HideDisplay()
    {
        currentDialogueInSequenceIndex = 0;
        TutorialHUD.SetActive(false);
    }
}
