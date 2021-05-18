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
    [SerializeField] private GameObject [] categoryObjects;

    private int currentCost = 100;
    private bool [] finishiedCategories;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        finishiedCategories = new bool[5];
        for(int i = 0; i < finishiedCategories.Length; i++)
        {
            finishiedCategories[i] = false;
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

    public void FinishCategory(int category)
    {
        if(CanFinish())
        {
            ResourceController.Instance.SpendCopper(currentCost);
            finishiedCategories[category] = true;
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
        foreach (GameObject category in categoryObjects)
        {
            if (!CanFinish())
                category.GetComponentInChildren<Button>().interactable = false;
            else
                category.GetComponentInChildren<Button>().interactable = true;

        }
    }

    public enum ProgressCategory
    {
        Category1,
        Category2,
        Category3,
        Category4,
        Category5
    }
}
