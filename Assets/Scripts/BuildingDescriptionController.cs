﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingDescriptionController : MonoBehaviour
{
    public static BuildingDescriptionController Instance = null;

    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI bonusesText;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        foreach(Building building in FindObjectsOfType<Building>())
        {
            building.OnClick += UpdateUI;
        }

    }

    public void AddToEvent(Building building)
    {
        building.OnClick += UpdateUI;
    }

    public void UpdateUI(BuildingEventData buildingEventData)
    {
        buildingNameText.text = buildingEventData.buildingName;
        categoryText.text = buildingEventData.category;
        descriptionText.text = buildingEventData.description;
        bonusesText.text = buildingEventData.bonuses;
    }
}
