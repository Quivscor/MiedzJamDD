﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    //Map of boosts and building Ids
    [SerializeField] protected Dictionary<string, int> m_neighborBoosts;
    public Dictionary<string, int> NeighborBoosts => m_neighborBoosts;

    [SerializeField] protected BuildingData m_buildingData;
    //make sure to use the same names here and in building data, otherwise fuckup incoming
    [SerializeField] protected string m_buildingID;
    public string BuildingID => m_buildingID;
    public List<Vector2Int> BuildingNeighbors => m_buildingData.neighborCoordinatesRelative;

    public bool IsPlaced { get; set; }

    protected int m_baseScore;
    protected int m_pointScore;
    public int PointScore => m_pointScore;
    //if different than 0, should be displayed when placing down
    protected int m_pointScoreDelta;
    public int PointScoreDelta => m_pointScoreDelta;

    #region Events
    public Action OnClick;
    #endregion

    private void Start()
    {
        IsPlaced = false;
        m_neighborBoosts = m_buildingData.GetNeighborBoostsData();
        m_buildingID = m_buildingData.thisBuildingID;
        m_baseScore = m_buildingData.pointValue;
    }

    public void RecalculatePointScore(Vector2Int ownCoords, List<Vector2Int> neighborCoords)
    {
        m_pointScoreDelta = m_pointScore;

        m_pointScore = m_baseScore + GetBonusFromNeighbors(ownCoords, neighborCoords);
        m_pointScoreDelta = Mathf.Abs(m_pointScore - m_pointScoreDelta);
    }

    //Check neighbors on placement to determine boosts
    private int GetBonusFromNeighbors(Vector2Int ownCoords, List<Vector2Int> neighborCoords)
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
            bool canBeBoosted = NeighborBoosts.TryGetValue(field.Building.m_buildingID, out value);
            result += value;
        }
        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CityDirector.Instance.TrySelectingBuilding(this);
    }
}