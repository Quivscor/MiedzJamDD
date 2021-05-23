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
            UpdateCosts();
            UpdateViability();
        }
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
