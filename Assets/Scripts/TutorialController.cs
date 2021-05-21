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

    public void DisplaySequence()
    {
        //Here be UI shenanigans

        currentSequenceIndex++;
    }    

    public void ButtonNext()
    {

    }

    public void HideDisplay()
    {
        currentDialogueInSequenceIndex = 0;
        //Here be UI shenanigans
    }
}
