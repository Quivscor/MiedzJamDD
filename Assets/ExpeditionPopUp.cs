using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ExpeditionPopUp : MonoBehaviour
{
    public static ExpeditionPopUp Instance = null;
    public TextMeshProUGUI copperText;
    public TextMeshProUGUI monthsText;
    public Button acceptExpeditionButton;
    public GameObject holder;
    private ExpeditionMap.Field curentField;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }


    public void SetupFieldInfo(ExpeditionMap.Field field, int currentCopper, int expectedTime)
    {
        curentField = field;
        copperText.text = CalculateExpectedCopper(currentCopper).ToString() + " miedzi";
        monthsText.text = expectedTime.ToString() + " miesięcy";
        holder.SetActive(true);
    }

    public void AcceptExpedition()
    {
        curentField.CreateExpeditionToThisField();
        holder.SetActive(false);

    }

    private int CalculateExpectedCopper(int currentCopper)
    {
        return (int)Mathf.Min((int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Transport].level * TeamStatsModifiers.LoadModifier, currentCopper);

    }


}
