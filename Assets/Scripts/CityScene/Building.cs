using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerClickHandler
{
    //Map of boosts and building Ids
    [SerializeField] protected Dictionary<string, float> m_neighborBoosts;
    public Dictionary<string, float> NeighborBoosts => m_neighborBoosts;

    [SerializeField] protected BuildingData m_buildingData;
    //make sure to use the same names here and in building data, otherwise fuckup incoming
    [SerializeField] protected string m_buildingID;
    public string BuildingID => m_buildingID;

    #region Events
    public Action OnClick;
    #endregion

    private void Start()
    {
        m_neighborBoosts = m_buildingData.GetNeighborBoostsData();

        //Check neighbors on placement to determine boosts
        CheckNeighbors();
    }

    public void CheckNeighbors()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("klikd");
        OnClick?.Invoke();
    }
}