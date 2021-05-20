using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingDescriptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI bonusesText;

    private void Start()
    {
        foreach(Building building in FindObjectsOfType<Building>())
        {
            building.OnClick += UpdateUI;
        }

    }

    public void UpdateUI()
    {
        buildingNameText.text = "Energol";
        categoryText.text = "Energetyka";
        descriptionText.text = "Fajnie świeci i daje dużo prądu";
        bonusesText.text = "- Bonus +1/n-Bonus +2";
    }
}
