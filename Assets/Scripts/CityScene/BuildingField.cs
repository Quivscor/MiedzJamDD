using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using TMProText = TMPro.TextMeshProUGUI;

public class BuildingField : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private MeshRenderer m_renderer;
    public MeshRenderer Renderer => m_renderer;
    private TMProText text;

    protected bool m_isEmpty = true;
    public bool IsEmpty => m_isEmpty;

    protected Vector2Int m_cityGridCoords;
    public Vector2Int CityGridCoordinates { get => m_cityGridCoords; set => m_cityGridCoords = value; }

    protected Building m_building;
    public Building Building => m_building;

    #region Events
    public Action<BuildingFieldEventData> OnClick;
    public Action<BuildingFieldEventData> OnHoverEnter;
    public Action<BuildingFieldEventData> OnHoverExit;
    #endregion

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
        text = GetComponentInChildren<TMProText>();
        text.gameObject.SetActive(false);
    }

    public void AssignBuilding(Building b)
    {
        m_building = b;
        m_isEmpty = false;
    }

    //Gets relative position based on this fields position
    public Vector2Int[] GetDefaultNeighbors()
    {
        Vector2Int[] result = new Vector2Int[4];
        //Northern neighbor
        if (m_cityGridCoords.y > 0)
            result[0] = new Vector2Int(0, 1);
        else
            result[0] = Vector2Int.zero;

        //Southern neighbor
        if (m_cityGridCoords.y < CityDirector.CityGridSize - 1)
            result[1] = new Vector2Int(0, -1);
        else
            result[1] = Vector2Int.zero;

        //Western neighbor
        if (m_cityGridCoords.x > 0)
            result[2] = new Vector2Int(0, -1);
        else
            result[2] = Vector2Int.zero;

        //Eastern neighbor
        if (m_cityGridCoords.x < CityDirector.CityGridSize - 1)
            result[3] = new Vector2Int(0, 1);
        else
            result[3] = Vector2Int.zero;

        return result;
    }

    public List<Vector2Int> GetNeighborsFromBuildingData()
    {
        List<Vector2Int> originalNeighbors = CityDirector.Instance.SelectedBuilding.SelectedBuilding.BuildingNeighbors;
        List<Vector2Int> result = new List<Vector2Int>(originalNeighbors);

        //need to zero all illegal placements
        for(int i = 0; i < result.Count; i++)
        {
            if (m_cityGridCoords.y + result[i].y < 0)
                result[i] = Vector2Int.zero;

            if (m_cityGridCoords.y + result[i].y >= CityDirector.CityGridSize)
                result[i] = Vector2Int.zero;

            if (m_cityGridCoords.x + result[i].x < 0)
                result[i] = Vector2Int.zero;

            if (m_cityGridCoords.x + result[i].x >= CityDirector.CityGridSize)
                result[i] = Vector2Int.zero;
        }

        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BuildingFieldEventData bEventData = new BuildingFieldEventData(this.transform.position, this, GetNeighborsFromBuildingData());
        OnClick?.Invoke(bEventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CityDirector.Instance.SelectedBuilding.SelectedBuilding == null)
            return;

        CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.transform.position = transform.position;
        BuildingFieldEventData bEventData = new BuildingFieldEventData(this.transform.position, this, GetNeighborsFromBuildingData());
        OnHoverEnter?.Invoke(bEventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CityDirector.Instance.SelectedBuilding.SelectedBuilding == null)
            return;

        CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.transform.position = LastSelectedBuilding.mockHidingPosition;
        BuildingFieldEventData bEventData = new BuildingFieldEventData(this.transform.position, this, GetNeighborsFromBuildingData(), CityDirector.Instance.SelectedBuilding.SelectedBuilding.BuildingCategory);
        OnHoverExit?.Invoke(bEventData);
    }

    public void DisplayInfo(int boostValue, int baseValue = 0)
    {
        if (boostValue == 0 && baseValue == 0)
            return;

        text.gameObject.SetActive(true);
        if (baseValue != 0 && boostValue != 0)
            text.text = baseValue.ToString() + "<color=green> + " + boostValue.ToString() + "</color>";
        else if (baseValue != 0 && boostValue == 0)
            text.text = baseValue.ToString();
        else if (boostValue != 0)
            text.text = "<color=green>+ " + boostValue.ToString() + "</color>";
        //if (baseValue > 0)
        //    text.text = baseValue.ToString() + " + " + boostValue.ToString();
        //else if(boostValue > 0)
        //    text.text = "+" + boostValue.ToString();
    }

    public void HideDisplay()
    {
        text.gameObject.SetActive(false);
    }
}

public class BuildingFieldEventData
{
    public BuildingFieldEventData() { /*default constructor for construction cleanup*/ }

    public BuildingFieldEventData(Vector3 pos, BuildingField selfRef)
    {
        fieldPosition = pos;
        selfReference = selfRef;
    }

    public BuildingFieldEventData(Vector3 pos, BuildingField selfRef, List<Vector2Int> neighborCoords)
    {
        fieldPosition = pos;
        selfReference = selfRef;
        neighborCoordsRelative = neighborCoords;
    }

    public BuildingFieldEventData(Vector3 pos, BuildingField selfRef, List<Vector2Int> neighborCoords, CategoriesProgressController.ScienceCategory category)
    {
        fieldPosition = pos;
        selfReference = selfRef;
        neighborCoordsRelative = neighborCoords;
        scienceCategory = category;
    }

    public Vector3 fieldPosition;
    public BuildingField selfReference;
    public List<Vector2Int> neighborCoordsRelative;
    public CategoriesProgressController.ScienceCategory scienceCategory;
}
