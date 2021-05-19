using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    //Map of boosts and building Ids
    [SerializeField] protected Dictionary<string, float> m_neighborBoosts;
    public Dictionary<string, float> NeighborBoosts => m_neighborBoosts;

    [SerializeField] protected BuildingData m_buildingData;
    //make sure to use the same names here and in building data, otherwise fuckup incoming
    [SerializeField] protected string m_buildingID;
    public string BuildingID => m_buildingID;
    public List<Vector2Int> BuildingNeighbors => m_buildingData.neighborCoordinatesRelative;

    public bool IsPlaced { get; set; }

    protected float m_pointScore;
    public float PointScore => PointScore;
    //if different than 0, should be displayed when placing down
    protected float m_pointScoreDelta;
    public float PointScoreDelta => m_pointScoreDelta;

    #region Events
    public Action OnClick;
    #endregion

    private void Start()
    {
        IsPlaced = false;
        m_neighborBoosts = m_buildingData.GetNeighborBoostsData();
        m_buildingID = m_buildingData.thisBuildingID;
    }

    //Check neighbors on placement to determine boosts
    public void CheckNeighbors(Vector2Int ownCoords, Vector2Int[] neighborCoords)
    {
        for(int i = 0; i < neighborCoords.Length; i++)
        {
            //if points at itself (basically a skip)
            if (neighborCoords[i] == Vector2Int.zero)
                continue;

            float value = 1;
            bool canBeBoosted = NeighborBoosts.TryGetValue(CityDirector.Instance.CityGrid[ownCoords.x + neighborCoords[i].x, ownCoords.y + neighborCoords[i].y].Building.m_buildingID, out value);
            if (canBeBoosted)
            {

            }
        }
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