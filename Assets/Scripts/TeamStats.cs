using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamStats : MonoBehaviour
{
    public static TeamStats Instance = null;

    [Header("UI:")]
    public TextMeshProUGUI maxLoadText = null;
    public TextMeshProUGUI maxDistance = null;
    public TextMeshProUGUI chanceToDiscover = null;

    private void Awake()
    {
        if (TeamStats.Instance == null)
            TeamStats.Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        CategoriesProgressController.Instance.OnCategoryLevelUp += OnCategoryLevelUp;

        maxDistance.text = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Energetics].level * TeamStatsModifiers.DistanceModifier + "";
        maxLoadText.text = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Transport].level * TeamStatsModifiers.LoadModifier + "";
        chanceToDiscover.text = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Telecommunication].level * TeamStatsModifiers.CommunicationModifier + "%";
    }

    public int AvailableDistance = 1;
    public int MaximumLoad = 1;

    public float ChanceToDiscoverAdditionalNeighbourField = 25.0f;

    public void OnCategoryLevelUp(CategoriesProgressController.ScienceCategory category)
    {
        if (category == CategoriesProgressController.ScienceCategory.Energetics)
            maxDistance.text = (int)CategoriesProgressController.Instance.sciences[(int)category].level * TeamStatsModifiers.DistanceModifier + "";

        if (category == CategoriesProgressController.ScienceCategory.Transport)
            maxLoadText.text = (int)CategoriesProgressController.Instance.sciences[(int)category].level * TeamStatsModifiers.LoadModifier + "";

        if (category == CategoriesProgressController.ScienceCategory.Telecommunication)
            chanceToDiscover.text = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Telecommunication].level * TeamStatsModifiers.CommunicationModifier + "%";
    }
}
