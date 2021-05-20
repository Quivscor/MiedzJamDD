using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSelectedBuilding : MonoBehaviour
{
    [SerializeField] private Building m_selectedBuilding;
    public Building SelectedBuilding => m_selectedBuilding;

    public static Vector3 mockHidingPosition = new Vector3(0f, 1000f, 0f);

    private Building m_selectedBuildingMock;
    public Building SelectedBuildingMock => m_selectedBuildingMock;

    public void SetBuilding(Building b)
    {
        if (b.IsPlaced)
            return;

        m_selectedBuilding = b;
        if (m_selectedBuildingMock != null)
            Destroy(m_selectedBuildingMock.gameObject);
        m_selectedBuildingMock = Instantiate(m_selectedBuilding, mockHidingPosition, Quaternion.identity, this.transform);
        m_selectedBuildingMock.gameObject.layer = CityDirector.IgnoreCameraRaycastLayerID;
        //m_selectedBuildingMock.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
    }

    public void Deselect()
    {
        m_selectedBuilding = null;
    }
}
