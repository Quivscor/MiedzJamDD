using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class EventController : MonoBehaviour
{
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
    private bool eventFiredThisWeek = false;

    private void Awake()
    {
        currentLibrary = new List<GameEventData>(gameEventLibrary);
    }

    private void Start()
    {
        TimeController.Instance.OnFirstCommonDay += WeekStartRandomizeSeed;
        TimeController.Instance.OnEndOfDay += TryGetEvent;

        WeekStartRandomizeSeed();
    }

    public void WeekStartRandomizeSeed()
    {
        if (currentLibrary.Count == 0)
            currentLibrary = new List<GameEventData>(gameEventLibrary);

        weekRandomSeed = Random.Range(0, 6);
        eventFiredThisWeek = false;
    }

    public void TryGetEvent(int dayOfTheWeek)
    {
        if(dayOfTheWeek >= weekRandomSeed && !eventFiredThisWeek)
        {
            int randomEventIndex = Random.Range(0, currentLibrary.Count);
            currentEventData = currentLibrary[randomEventIndex];
            DisplayEvent();
            currentLibrary.RemoveAt(randomEventIndex);
            eventFiredThisWeek = true;
        }
    }

    public void DisplayEvent()
    {
        eventHUD.SetActive(true);
        titleObject.text = currentEventData.eventTitle;
        descriptionObject.text = currentEventData.eventText;

        for(int i = 0; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(true);
            buttonRefs[i].GetComponentInChildren<TMProText>().text = currentEventData.eventChoices[i].choiceText;
        }
    }

    public void ButtonAction(int choice)
    {
        if (!nextButtonActionCloseEvent)
            nextButtonActionCloseEvent = true;
        else
            HideEvent();

        buttonRefs[0].GetComponentInChildren<TMProText>().text = defaultCloseWindowText;
        for(int i = 1; i < currentEventData.eventChoices.Count; i++)
        {
            buttonRefs[i].SetActive(false);
        }

        descriptionObject.text = currentEventData.eventChoices[choice].choiceText;

        //currentEventData.eventChoices[choice].currencyReward
        //currentEventData.eventChoices[choice].packageReward
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
