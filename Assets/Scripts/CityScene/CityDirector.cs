using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityDirector : MonoBehaviour
{
    public static CityDirector Instance;

    public static int CityGridSize = 10;
    //turn this layer off in physics raycast in camera, so building can't be selected after placing
    public static int IgnoreCameraRaycastLayerID = 8;
    
    private readonly BuildingField[,] m_cityGrid = new BuildingField[CityGridSize, CityGridSize];

    public BuildingField[,] CityGrid { get => m_cityGrid; }

    protected LastSelectedBuilding m_lastSelectedBuilding;
    public LastSelectedBuilding SelectedBuilding => m_lastSelectedBuilding;
    protected ComboDisplayerComponent m_comboDisplayerComponent;

    #region Setup
    [Header("Scene setup")]
    public GameObject buildingFieldPrefab;
    #endregion

    #region Events
    public Action<CityDirectorEventData> OnBuildingPlaced;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        int roadX = 3;
        int roadX2 = 8;
        int roadZ = 6;

        float extraX = 0, extraZ = 0;

        //placing building fields
        for (int x = 0; x < CityGridSize; x++)
        {
            if (x >= roadX)
                extraX = .5f;
            else
                extraX = 0;

            if (x >= roadX2)
                extraX += .5f;

            for (int z = 0; z < CityGridSize; z++)
            {
                //DODAC DROGI
                if (z >= roadZ)
                    extraZ = .5f;
                else
                    extraZ = 0;

                BuildingField fieldGO = Instantiate(buildingFieldPrefab, this.transform.position + new Vector3(x + extraX + 0.5f, 0, z + extraZ + 0.5f), Quaternion.identity, this.transform).GetComponentInChildren<BuildingField>();
                fieldGO.gameObject.name = "Field (" + x + ", " + z + ")";
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

    public void DeselectCurrentBuilding()
    {
        m_lastSelectedBuilding.Deselect();
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

        b.transform.parent = null;
        b.transform.position = eventData.fieldPosition;
        b.IsPlaced = true;
        //turn this layer off in physics raycast in camera
        b.gameObject.layer = IgnoreCameraRaycastLayerID;
        eventData.selfReference.AssignBuilding(b);

        //recalculate grid and apply extra points to points controller
        List<Building> buildingsGivingPoints = new List<Building>();
        buildingsGivingPoints.Add(b);
        b.RecalculatePointScore(eventData.selfReference.CityGridCoordinates, eventData.selfReference.GetNeighborsFromPlacedBuilding());
        CategoriesProgressController.Instance.AddPointsToScience(b.BuildingCategory, b.PointScoreDelta);
        RecalculateHighlightedNeighbors(eventData.selfReference.GetNeighborsFromBuildingData(false), eventData.selfReference.CityGridCoordinates, ref buildingsGivingPoints);

        //if needed some info from city director here, edit the class and add what's needed
        OnBuildingPlaced?.Invoke(new CityDirectorEventData(b));

        b.ProcessPlacingInAnimation();

        m_lastSelectedBuilding.Deselect();
        m_comboDisplayerComponent.CleanupDisplay(eventData);
    }

    public void RecalculateHighlightedNeighbors(List<Vector2Int> coords, Vector2Int owncoords, ref List<Building> listToFill)
    {
        foreach(Vector2Int coord in coords)
        {
            if (CityGrid[coord.x, coord.y].Building == null || owncoords == coord)
                continue;

            Building b = CityGrid[coord.x, coord.y].Building;
            int value = 0;
            b.NeighborBoosts.TryGetValue(SelectedBuilding.SelectedBuildingMock.BuildingID, out value);
            listToFill.Add(b);
            CategoriesProgressController.Instance.AddPointsToScience(b.BuildingCategory, value);
        }
    }

    public void RecalculateGrid(bool updateScore = false)
    {
        int debugScore = 0;
        for(int i = 0; i < CityGridSize; i++)
        {
            for(int j = 0; j < CityGridSize; j++)
            {
                if (CityGrid[i, j].Building == null)
                    continue;

                CityGrid[i, j].Building.RecalculatePointScore(new Vector2Int(i, j), CityGrid[i, j].GetNeighborsFromPlacedBuilding());

                if (updateScore)
                    CategoriesProgressController.Instance.AddPointsToScience(CityGrid[i, j].Building.BuildingCategory, CityGrid[i, j].Building.PointScoreDelta);
                debugScore += CityGrid[i, j].Building.PointScore;
            }
        }

        Debug.Log("Grid score = " + debugScore);
    }
}

public class CityDirectorEventData
{
    public CityDirectorEventData() { }

    public CityDirectorEventData(Building building)
    {
        placedBuilding = building;
    }

    public Building placedBuilding;
}
