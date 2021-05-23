using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance = null;

    [SerializeField] private Animator copperAnimator;
    [SerializeField] private Animator copperDropDownAnimator;
    [SerializeField] private TextMeshProUGUI copperDropDownText;
    [SerializeField] private TextMeshProUGUI copperText;
    [SerializeField] private int startingCopper;
    private int copper;
    private int lifeTimeCopper = 0;

    public int LifetimeCopper => lifeTimeCopper;
    public int Copper { get => copper; }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        copper = startingCopper;
        UpdateUI();
    }

    public void AddCopper(int value)
    {
        copper += value;
        lifeTimeCopper += value;
        copperAnimator.SetTrigger("Pulse");
        UpdateUI();
    }

    public void SpendCopper(int value)
    {
        copper -= value;
        copperDropDownText.text = "-" + value;
        copperDropDownAnimator.SetTrigger("DropDown");

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
