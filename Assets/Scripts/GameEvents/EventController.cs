using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class EventController : MonoBehaviour
{
    public static EventController Instance;

    public GameEventData currentEventData;

    public Animator inventoryAnimator;
    public GameObject eventHUD;
    public TMProText titleObject;
    public TMProText descriptionObject;
    public List<GameObject> buttonRefs;

    public string defaultCloseWindowText = "Zamknij";
    private bool nextButtonActionCloseEvent = false;

    public List<GameEventData> gameEventLibrary;
    public List<GameEventData> currentLibrary;
    private int weekRandomSeed;
    private int daysSinceEvent;
    private bool eventFiredThisWeek = false;

    public Action<int> OnGameEventFire;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        currentLibrary = new List<GameEventData>(gameEventLibrary);
        currentLibrary = currentLibrary.OrderBy(x => Guid.NewGuid()).ToList(); //something from the web
    }

    private void Start()
    {
        //TimeController.Instance.OnFirstCommonDay += WeekStartRandomizeSeed;
        TimeController.Instance.OnEndOfDay += TryGetEvent;

        WeekStartRandomizeSeed();
    }

    public void WeekStartRandomizeSeed()
    {
        if (currentLibrary.Count == 0)
        {
            currentLibrary = new List<GameEventData>(gameEventLibrary);
            currentLibrary = currentLibrary.OrderBy(x => Guid.NewGuid()).ToList(); //something from the web
        }
            
        weekRandomSeed = Random.Range(7, 10); //days from last event
        eventFiredThisWeek = false;
    }

    public void TryGetEvent(int dayOfTheWeek)
    {
        if(daysSinceEvent >= weekRandomSeed)
        {
            int randomEventIndex = Random.Range(0, currentLibrary.Count);
            currentEventData = currentLibrary[randomEventIndex];
            DisplayEvent();
            currentLibrary.RemoveAt(randomEventIndex);
            //eventFiredThisWeek = true;
            daysSinceEvent = 0;
        }
        else
        {
            daysSinceEvent++;
        }
    }

    public void DisplayEvent()
    {
        inventoryAnimator.SetTrigger("hideTrigger");
        OnGameEventFire?.Invoke(3); // game event tutorial plays here

        eventHUD.SetActive(true);
        titleObject.text = currentEventData.eventTitle;
        descriptionObject.text = currentEventData.eventText;

        for(int i = 0; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(true);
            if (currentEventData.eventChoices[i].choiceCost > ResourceController.Instance.Copper)
            {
                buttonRefs[i].GetComponent<Button>().interactable = false;
                buttonRefs[i].GetComponentInChildren<TMProText>().text = "<color=red>" + currentEventData.eventChoices[i].choiceCost + " miedzi</color>";
            }
            else
                buttonRefs[i].GetComponentInChildren<TMProText>().text = currentEventData.eventChoices[i].choiceButtonText;
        }
    }

    public void ButtonAction(int choice)
    {
        if (!nextButtonActionCloseEvent)
            nextButtonActionCloseEvent = true;
        else
        {
            HideEvent();
            return;
        }
            

        buttonRefs[0].GetComponentInChildren<TMProText>().text = defaultCloseWindowText;
        buttonRefs[0].GetComponent<Button>().interactable = true;
        for(int i = 1; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(false);
        }

        descriptionObject.text = currentEventData.eventChoices[choice].choiceText;

        if (currentEventData.eventChoices[choice].currencyReward > 0)
            ResourceController.Instance.AddCopper(currentEventData.eventChoices[choice].currencyReward);
        else if (currentEventData.eventChoices[choice].currencyReward < 0)
            ResourceController.Instance.SpendCopper(-1 * currentEventData.eventChoices[choice].currencyReward);

        if (currentEventData.eventChoices[choice].choiceCost > 0)
            ResourceController.Instance.SpendCopper(currentEventData.eventChoices[choice].choiceCost);

        //currentEventData.eventChoices[choice].packageReward

        CategoriesProgressController.Instance.AddPointsToScience(currentEventData.eventChoices[choice].category, currentEventData.eventChoices[choice].scienceCategoryScorePenalty);
        //currentEventData.eventChoices[choice].scienceCategoryScorePenalty
        //currentEventData.eventChoices[choice].category
    }

    public void HideEvent()
    {
        for (int i = 0; i < buttonRefs.Count; i++)
            buttonRefs[i].SetActive(false);
        nextButtonActionCloseEvent = false;
        eventHUD.SetActive(false);
        WeekStartRandomizeSeed();
        inventoryAnimator.SetTrigger("showTrigger");
    }
}

