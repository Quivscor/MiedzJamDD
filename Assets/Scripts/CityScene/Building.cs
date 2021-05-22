using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Building : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    public RuntimeAnimatorController mainAnimatorController = null;
    public RuntimeAnimatorController personalAnimatorController = null;
    //Map of boosts and building Ids
    [SerializeField] protected Dictionary<string, int> m_neighborBoosts;
    public Dictionary<string, int> NeighborBoosts => m_neighborBoosts;

    [SerializeField] protected BuildingData m_buildingData;
    //make sure to use the same names here and in building data, otherwise fuckup incoming
    public string BuildingID => m_buildingData.thisBuildingID;
    public List<Vector2Int> BuildingNeighbors => m_buildingData.neighborCoordinatesRelative;
    public CategoriesProgressController.ScienceCategory BuildingCategory => m_buildingData.pointCategory;

    public bool IsPlaced { get; set; }

    protected int m_baseScore;
    public int BaseScore => m_baseScore;
    protected int m_pointScore;
    public int PointScore => m_pointScore;
    //if different than 0, should be displayed when placing down
    protected int m_pointScoreDelta;
    public int PointScoreDelta => m_pointScoreDelta;

    #region Events
    public Action<BuildingEventData> OnClick;
    #endregion

    private void Start()
    {
        IsPlaced = false;
        m_neighborBoosts = m_buildingData.GetNeighborBoostsData();
        m_baseScore = m_buildingData.pointValue;
    }

    public void RecalculatePointScore(Vector2Int ownCoords, List<Vector2Int> neighborCoords)
    {
        m_pointScoreDelta = m_pointScore;

        m_pointScore = m_baseScore + GetBonusFromNeighbors(ownCoords, neighborCoords);
        m_pointScoreDelta = Mathf.Abs(m_pointScore - m_pointScoreDelta);
    }

    //Check neighbors on placement to determine boosts
    public int GetBonusFromNeighbors(Vector2Int ownCoords, List<Vector2Int> neighborCoords)
    {
        int result = 0;
        for(int i = 0; i < neighborCoords.Count; i++)
        {
            //if points at itself (basically a skip)
            if (neighborCoords[i] == Vector2Int.zero)
                continue;

            BuildingField field = CityDirector.Instance.CityGrid[ownCoords.x + neighborCoords[i].x, ownCoords.y + neighborCoords[i].y];
            if (field.Building == null)
                continue;

            int value = 0;
            bool canBeBoosted = NeighborBoosts.TryGetValue(field.Building.BuildingID, out value);
            result += value;
        }
        return result;
    }

    //for display purposes, check if given neighbor gives boost and return just his boost
    public int GetBonusFromSpecificNeighbor(Vector2Int neighborCoords)
    {
        BuildingField field = CityDirector.Instance.CityGrid[neighborCoords.x, neighborCoords.y];
        if (field.Building == null)
            return int.MinValue;

        int value = 0;
        bool canBeBoosted = NeighborBoosts.TryGetValue(field.Building.BuildingID, out value);
        return value;
    }

    public int GetBonusFromNeighboringWithMock(Vector2Int coordsToCheck, Vector2Int ownCoords)
    {
        bool correctPlacement = false;
        foreach(Vector2Int coords in m_buildingData.neighborCoordinatesRelative)
        {
            if (coordsToCheck == ownCoords + coords)
                correctPlacement = true;
        }

        if (!correctPlacement)
            return 0;

        int value = 0;
        bool canBeBoosted = NeighborBoosts.TryGetValue(CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BuildingID, out value);
        return value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(new BuildingEventData(m_buildingData.thisBuildingID, m_buildingData.pointCategory, m_buildingData.description, BonusesStringGenerator()));
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CityDirector.Instance.TrySelectingBuilding(this);
    }

    public string BonusesStringGenerator()
    {
        string result = "";
        result = "- Wszystkich z " + m_buildingData.pointCategory + " <color=yellow>(+1)</color>\n";
        for (int i = 0; i < m_buildingData.buildingIDs.Count; i++)
        {
            // skipping building within same category
            if(m_buildingData.buildingBoosts[i] != 1)
                result += "- " + m_buildingData.buildingIDs[i] + " <color=yellow>(+" + m_buildingData.buildingBoosts[i] + ")</color>\n";
        }

        return result;
    }

    public void ProcessPlacingInAnimation(int points, PointsComboVisualManager visualManager)
    {
        this.visualManager = visualManager;

        addedPointsText.text = "+" + points;

        switch (BuildingCategory)
        {
            case CategoriesProgressController.ScienceCategory.Energetyka:
                addedPointsText.color = Color.yellow;
                break;
            case CategoriesProgressController.ScienceCategory.Telekomunikacja:
                addedPointsText.color = Color.magenta;
                break;
            case CategoriesProgressController.ScienceCategory.Transport:
                addedPointsText.color = Color.red;
                break;
            case CategoriesProgressController.ScienceCategory.Społeczność:
                addedPointsText.color = Color.green;
                break;
        }

        GetComponent<Animator>()?.SetTrigger("buildingPlacedTrigger");
    }

    private PointsComboVisualManager visualManager = null;
    private RuntimeAnimatorController currentControllerTmp = null;

    public TextMeshProUGUI addedPointsText = null;

    public void ShowVisualAddedPoints(int points, PointsComboVisualManager visualManager)
    {
        this.visualManager = visualManager;

        Animator animator = GetComponent<Animator>();

        currentControllerTmp = animator.runtimeAnimatorController;

        animator.runtimeAnimatorController = mainAnimatorController;

        addedPointsText.text = "+" + points;

        switch (BuildingCategory)
        {
            case CategoriesProgressController.ScienceCategory.Energetyka:
                addedPointsText.color = Color.yellow;
                break;
            case CategoriesProgressController.ScienceCategory.Telekomunikacja:
                addedPointsText.color = Color.magenta;
                break;
            case CategoriesProgressController.ScienceCategory.Transport:
                addedPointsText.color = Color.red;
                break;
            case CategoriesProgressController.ScienceCategory.Społeczność:
                addedPointsText.color = Color.green;
                break;
        }


        animator?.SetTrigger("addPointsTrigger");
    }

    #region Anim Events

    public void OnBuildingPlaced()
    {
        Animator animator = GetComponent<Animator>();

        if (animator && personalAnimatorController != null)
            animator.runtimeAnimatorController = personalAnimatorController;

        visualManager?.OnVisualAddingFinished();
    }

    public void OnPointsAdded()
    {
        GetComponent<Animator>().runtimeAnimatorController = currentControllerTmp;

        visualManager?.OnVisualAddingFinished();
    }

    #endregion
}