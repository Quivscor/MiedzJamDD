using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingField : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    protected bool m_isEmpty = true;
    public bool IsEmpty => m_isEmpty;

    protected Building m_building;
    public Building Building => m_building;

    #region Events
    public Action<BuildingFieldEventData> OnClick;
    #endregion

    public void AssignBuilding(Building b)
    {
        m_building = b;
        m_isEmpty = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(new BuildingFieldEventData(this.transform.position, this));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CityDirector.Instance.SelectedBuilding.SelectedBuilding == null)
            return;

        CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.transform.position = transform.position;
    }
}

public class BuildingFieldEventData
{
    public BuildingFieldEventData(Vector3 pos, BuildingField selfRef)
    {
        fieldPosition = pos;
        selfReference = selfRef;
    }

    public Vector3 fieldPosition;
    public BuildingField selfReference;
}
