using System.Collections;
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

    public bool IsWorking = true;

    public Building startingBuilding;

    private void Start()
    {
        if (!IsWorking)
            return;

        BuildingsInventory.Instance.OnEmptyInventory += DisplaySequence; //InventoryTutorial
        EarthProgressController.Instance.OnMissionsFromEarthWindowOpen += DisplaySequence; //MissionsFromEarthTutorial
        EventController.Instance.OnGameEventFire += DisplaySequence; //GameEventTutorial
        ExpeditionMap.ExpeditionManager.Instance.OnExpeditionConfirmation += DisplaySequence; //AfterExpeditionTutorial
        FindObjectOfType<PackagesInventory>().OnBuyPackage += DisplaySequence; //OnBuyingPackage
        ExpeditionMap.ExpeditionManager.Instance.OnReceiveRaport += DisplaySequence; //OnReceiveRaportTutorial

        startingBuilding = BuildingsInventory.Instance.SpawnBuilding(startingBuilding, 0);
        startingBuilding.OnSelectPlaySequence += DisplaySequence; //OnFirstBuildingSelected;

        DisplaySequence(0);
    }

    public void DisplaySequence(int sequenceID)
    {
        currentSequenceIndex = sequenceID;
        if (sequences[currentSequenceIndex].hasFired)
            return;

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
