using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EarthProgressController : MonoBehaviour
{
    public static EarthProgressController Instance = null;

    [SerializeField] private float percentageCostIncrease = 0.5f;
    [SerializeField] private TextMeshProUGUI [] costTexts;
    [SerializeField] private GameObject [] missionObjects;

    private int currentCost = 100;
    private bool [] finishedMissions;

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

    public void FinishMission(int category)
    {
        if(CanFinish())
        {
            ResourceController.Instance.SpendCopper(currentCost);
            finishedMissions[category] = true;
            currentCost += (int)(currentCost * percentageCostIncrease);
            UpdateCosts();
            UpdateViability();
        }
    }

    public void UpdateCosts()
    {
        foreach (TextMeshProUGUI text in costTexts)
        {
            text.text = "Koszt " + currentCost.ToString();
        }
    }
    public void UpdateViability()
    {
        foreach (GameObject mission in missionObjects)
        {
            if (!CanFinish())
                mission.GetComponentInChildren<Button>().interactable = false;
            else
                mission.GetComponentInChildren<Button>().interactable = true;

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
