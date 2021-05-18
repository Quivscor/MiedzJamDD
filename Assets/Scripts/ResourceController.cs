using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance = null;

    [SerializeField] private TextMeshProUGUI copperText;
    private int copper = 100000;

    public int Copper { get => copper; }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        UpdateUI();
    }

    public void AddCopper(int value)
    {
        copper += value;
        UpdateUI();
    }

    public void SpendCopper(int value)
    {
        copper -= value;

        if (copper < 0)
            Debug.LogError("COPPPER IS LESS THAN 0. CHECK IT AMOUNT BEFORE SPENDING");

        UpdateUI();
    }

    public void UpdateUI()
    {
        copperText.text = copper.ToString();
    }

    public bool HasEnoughCopper(int value)
    {
        if (copper - value >= 0)
            return true;
        else
        {
            Debug.Log("You don't have enough copper.");
            return false;
        }
    }

}
