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

    public bool IsPlaced { get; set; }

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
    public void CheckNeighbors()
    {
        
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