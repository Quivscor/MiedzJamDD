using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;
using System;
using Random = UnityEngine.Random;

public class EventController : MonoBehaviour
{
    public static EventController Instance;

    public GameEventData currentEventData;

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
            currentLibrary = new List<GameEventData>(gameEventLibrary);

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
        OnGameEventFire?.Invoke(3); // game event tutorial plays here

        eventHUD.SetActive(true);
        titleObject.text = currentEventData.eventTitle;
        descriptionObject.text = currentEventData.eventText;

        for(int i = 0; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(true);
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
        for(int i = 1; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(false);
        }

        descriptionObject.text = currentEventData.eventChoices[choice].choiceText;

        if (currentEventData.eventChoices[choice].currencyReward > 0)
            ResourceController.Instance.AddCopper(currentEventData.eventChoices[choice].currencyReward);
        else if (currentEventData.eventChoices[choice].currencyReward < 0)
            ResourceController.Instance.SpendCopper(currentEventData.eventChoices[choice].currencyReward);

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
    }
}
