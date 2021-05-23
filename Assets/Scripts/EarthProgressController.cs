using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EarthProgressController : MonoBehaviour
{
    public static EarthProgressController Instance = null;

    [SerializeField] private float costIncreaseMutliplier = 1f;
    [SerializeField] private TextMeshProUGUI [] costTexts;
    [SerializeField] private Image [] icons;
    [SerializeField] private GameObject [] redBars;
    [SerializeField] private GameObject [] missionObjects;

    private int currentCost = 100;
    private bool [] finishedMissions;

    public Action<int> OnMissionsFromEarthWindowOpen;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        finishedMissions = new bool[5];
        for(int i = 0; i < finishedMissions.Length; i++)
        {
            finishedMissions[i] = false;
            redBars[i].SetActive(false);
        }
    }

    public bool CanFinish()
    {
        if (ResourceController.Instance.HasEnoughCopper(currentCost))
        {
            return true;
        }
        else return false;
    }
    public void Refresh()
    {
        OnMissionsFromEarthWindowOpen?.Invoke(2); // this fires tutorial nr 2
        UpdateCosts();
        UpdateViability();
    }
    public void FinishMission(int category)
    {
        if(CanFinish())
        {
            ResourceController.Instance.SpendCopper(currentCost);
            finishedMissions[category] = true;
            currentCost *= (int)(costIncreaseMutliplier);
            CheckForEndGame();
            UpdateCosts();
            UpdateViability();
        }
    }


    public void CheckForEndGame()
    {
        for (int i = 0; i < finishedMissions.Length; i++)
        {
            if (finishedMissions[i] == false)
                return;
        }

        EndGame();
    }

    public void EndGame()
    {
        StartCoroutine(EndGameFade());
        Debug.Log("BRAWO WYGRAŁEŚ");
    }

    public IEnumerator EndGameFade()
    {
        SceneFader fader = FindObjectOfType<SceneFader>();
        StartCoroutine(fader.FadeOut());
        yield return new WaitForSeconds(1f);
        EndGameDisplay.Instance.DisplayGameEnd();
        StartCoroutine(fader.FadeIn());
    }

    public void UpdateCosts()
    {
        for (int i = 0; i < missionObjects.Length; i++)
        {
            if(!finishedMissions[i])
            {
                costTexts[i].text = "Zakończ (" + currentCost + ")";
            }
            else
                costTexts[i].text = "Zakończono";
        }

    }
    public void UpdateViability()
    {

        for (int i = 0; i < missionObjects.Length; i++)
        {
            if (!CanFinish() || finishedMissions[i])
            {
                missionObjects[i].GetComponentInChildren<Button>().interactable = false;
                redBars[i].SetActive(false);
                
            }
            else
                missionObjects[i].GetComponentInChildren<Button>().interactable = true;

            if(finishedMissions[i])
                icons[i].color = new Color(0.4235294f, 0.4235294f, 0.4235294f, 0.72f);
        }

    }

    public enum MissionType
    {
        Category1,
        Category2,
        Category3,
        Category4,
        Category5
    }
}
