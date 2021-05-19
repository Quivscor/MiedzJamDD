using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityDirector : MonoBehaviour
{
    public static CityDirector Instance;

    public static int CityGridSize = 8;
    //turn this layer off in physics raycast in camera, so building can't be selected after placing
    public static int IgnoreCameraRaycastLayerID = 8;
    
    private readonly BuildingField[,] m_cityGrid = new BuildingField[CityGridSize, CityGridSize];
    public BuildingField[,] CityGrid { get => m_cityGrid; }

    protected LastSelectedBuilding m_lastSelectedBuilding;
    public LastSelectedBuilding SelectedBuilding => m_lastSelectedBuilding;
    protected ComboDisplayerComponent m_comboDisplayerComponent;

    #region Setup
    [Header("Scene setup")]
    public BuildingField buildingFieldPrefab;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        //placing building fields
        for (int x = 0; x < CityGridSize; x++)
        {
            for (int z = 0; z < CityGridSize; z++)
            {
                BuildingField fieldGO = Instantiate(buildingFieldPrefab, this.transform.position + new Vector3(x + 0.5f, 0, z + 0.5f), Quaternion.identity, this.transform);
                fieldGO.name = "Field (" + x + ", " + z + ")";
                fieldGO.OnClick += TryConfirmBuilding;
                fieldGO.CityGridCoordinates = new Vector2Int(x, z);
                m_cityGrid[x, z] = fieldGO;
            }
        }

        m_lastSelectedBuilding = GetComponent<LastSelectedBuilding>();
        m_comboDisplayerComponent = GetComponent<ComboDisplayerComponent>();
    }

    public void TrySelectingBuilding(Building b)
    {
        m_lastSelectedBuilding.SetBuilding(b);
    }

    public void TryConfirmBuilding(BuildingFieldEventData eventData)
    {
        if (m_lastSelectedBuilding.SelectedBuilding == null)
        {
            Debug.Log("Nothing selected");
            return;
        }

        if(!eventData.selfReference.IsEmpty)
        {
            Debug.Log("Field taken");
            return;
        }
        Building b = m_lastSelectedBuilding.SelectedBuilding;

        b.transform.position = eventData.fieldPosition;
        b.IsPlaced = true;
        //turn this layer off in physics raycast in camera
        b.gameObject.layer = IgnoreCameraRaycastLayerID;
        eventData.selfReference.AssignBuilding(b);

        //checking and applying neighborhood boosts
        //b.CheckNeighbors(eventData.selfReference.CityGridCoordinates, eventData.selfReference.GetDefaultNeighbors());

        m_lastSelectedBuilding.Deselect();
        m_comboDisplayerComponent.CleanupDisplay(new BuildingFieldEventData());
    }
}
