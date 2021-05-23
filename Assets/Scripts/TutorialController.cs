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
    public Image blackBG;

    public bool IsWorking = true;

    public Building startingBuilding;
    public Building startingBuilding2;

    public Button doEkspedycji;
    public Button doMiasta;
    public Button sklep;
    public Button paczka;
    public Button zakonczMiesiac;
    public Button misjeZZiemi;

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
        startingBuilding2 = BuildingsInventory.Instance.SpawnBuilding(startingBuilding2, 1);
        startingBuilding2.OnSelectPlaySequence += DisplaySequence; //OnFirstBuildingSelected;

        FindObjectOfType<SceneFader>().OnCityTransition += DisplaySequence; //OnCityTransition tutorial

        DisplaySequence(0);
    }

    public void DisplaySequence(int sequenceID)
    {
        if (sequenceID != 0)
            blackBG.color = new Color(0, 0, 0, 0);

        currentSequenceIndex = sequenceID;
        if (sequences[currentSequenceIndex].hasFired)
            return;

        doEkspedycji.interactable = sequences[currentSequenceIndex].doEkspedycji;
        doMiasta.interactable = sequences[currentSequenceIndex].doMiasta;
        sklep.interactable = sequences[currentSequenceIndex].sklep;
        paczka.interactable = sequences[currentSequenceIndex].paczka;
        zakonczMiesiac.interactable = sequences[currentSequenceIndex].zakonczMiesiac;
        misjeZZiemi.interactable = sequences[currentSequenceIndex].misjeZZiemi;

        sequences[currentSequenceIndex].hasFired = true;

        TutorialHUD.SetActive(true);
        DisplayDialogue();
    }    

    public void DisplayDialogue()
    {
        dialogueText.text = sequences[currentSequenceIndex].dialogues[currentDialogueInSequenceIndex].dialogueMsg;
        if (sequences[currentSequenceIndex].dialogues[currentDialogueInSequenceIndex].dialogueSprite == null)
            tutorialLadyImage.color = new Color(0, 0, 0, 0);
        else
            tutorialLadyImage.color = Color.white;
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
        bool next = false;
        if (sequences[currentSequenceIndex].chainSequenceID > -1)
            next = true;

        currentDialogueInSequenceIndex = 0;
        TutorialHUD.SetActive(false);

        doEkspedycji.interactable = true;
        doMiasta.interactable = true;
        sklep.interactable = true;
        paczka.interactable = true;
        zakonczMiesiac.interactable = true; 
        misjeZZiemi.interactable = true;

        if (next)
            DisplaySequence(sequences[currentSequenceIndex].chainSequenceID);
    }
}
