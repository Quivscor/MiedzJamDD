using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDisplayerComponent : MonoBehaviour
{
    private List<BuildingField> displayedFields;
    private List<BuildingField> fieldsToCleanup;

    public Material defaultBuildingFieldMaterial;
    public Material boostedField;
    public Material otherComboField;

    private void Start()
    {
        boostedField = new Material(boostedField);
        defaultBuildingFieldMaterial = new Material(defaultBuildingFieldMaterial);

        displayedFields = new List<BuildingField>();

        for (int i = 0; i < CityDirector.CityGridSize; i++)
        {
            for(int j = 0; j < CityDirector.CityGridSize; j++)
            {
                CityDirector.Instance.CityGrid[i, j].OnHoverEnter += TryDisplayComboPotential;
                CityDirector.Instance.CityGrid[i, j].OnHoverExit += CleanupDisplay;
            }
        }
    }

    //this is called first
    public void TryDisplayComboPotential(BuildingFieldEventData eventData)
    {
        displayedFields = new List<BuildingField>();
        //get displayedFields full of refs to required objects
        for(int i = 0; i < eventData.neighborCoordsRelative.Count; i++)
        {
            //we skip zeros
            if (eventData.neighborCoordsRelative[i] == Vector2Int.zero)
                continue;

            
            displayedFields.Add(CityDirector.Instance.CityGrid[eventData.selfReference.CityGridCoordinates.x + eventData.neighborCoordsRelative[i].x,
                eventData.selfReference.CityGridCoordinates.y + eventData.neighborCoordsRelative[i].y]);
        }

        //save the list to cleanup list, because this will be overwritten first
        fieldsToCleanup = new List<BuildingField>(displayedFields);

        //change material so they're more visible
        HighlightBuildingFields();

        //display numerical values of boosts above fields
        DisplayNumericalBoosts(eventData.selfReference);
    }

    public void HighlightBuildingFields()
    {
        foreach(BuildingField f in displayedFields)
        {
            f.Renderer.enabled = true;
            //f.Renderer.material = boostedField;
        }
    }

    public void HighlightOtherBuildingComboRanges(Vector2Int hoverPosition)
    {
        foreach (BuildingField f in displayedFields)
        {
            if (f.Building == null)
                continue;

            List<Vector2Int> neighborCoords = f.GetNeighborsFromPlacedBuilding(false);
            bool shouldDisplay = false;
            foreach(Vector2Int coords in neighborCoords)
            {
                //does it combo with hovered building
                int value = 0; //not important
                if (!f.Building.NeighborBoosts.TryGetValue(CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BuildingID, out value))
                    continue;
                if (coords != hoverPosition)
                    continue;
                shouldDisplay = true;
                break;
            }
            if(shouldDisplay)
            {
                foreach (Vector2Int coords in neighborCoords)
                {
                    CityDirector.Instance.CityGrid[coords.x, coords.y].Renderer.material = otherComboField;
                    fieldsToCleanup.Add(CityDirector.Instance.CityGrid[coords.x, coords.y]);
                }
            }
        }
    }

    public void DisplayNumericalBoosts(BuildingField pointedField)
    {
        CategoriesProgressController.ScienceCategory[] categories = new CategoriesProgressController.ScienceCategory[4];
        int[] categoryPoints = new int[4];

        categories[0] = CategoriesProgressController.ScienceCategory.Energetyka;
        categories[1] = CategoriesProgressController.ScienceCategory.Telekomunikacja;
        categories[2] = CategoriesProgressController.ScienceCategory.Transport;
        categories[3] = CategoriesProgressController.ScienceCategory.Społeczność;

        int extra = CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.GetBonusFromNeighbors(pointedField.CityGridCoordinates, pointedField.GetNeighborsFromBuildingData());
        pointedField.DisplayInfo(extra, BuildingEventData.GetCategoryColor(CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BuildingCategory) ,CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BaseScore);
        categoryPoints[(int)CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BuildingCategory] += CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BaseScore + extra;

        foreach (BuildingField f in displayedFields)
        {
            if (f.Building == null)
                continue;
            int value = 0;
            f.Building.NeighborBoosts.TryGetValue(CityDirector.Instance.SelectedBuilding.SelectedBuildingMock.BuildingID, out value);
            f.DisplayInfo(value, BuildingEventData.GetCategoryColor(f.Building.BuildingCategory));
            categoryPoints[(int)f.Building.BuildingCategory] += value;
        }

        for (int i = 0; i < categoryPoints.Length; i++)
        {
            CategoriesProgressController.Instance.AddPointsToScienceHover(categories[i], categoryPoints[i]);
        }
    }

    //this is called after TryDisplayComboPotential
    public void CleanupDisplay(BuildingFieldEventData eventData)
    {
        eventData.selfReference.HideDisplay();
        foreach (BuildingField f in fieldsToCleanup)
        {
            f.Renderer.enabled = false;
            //f.Renderer.material = defaultBuildingFieldMaterial;
            f.HideDisplay();
        }
        CategoriesProgressController.Instance.StopHover();
    }
}
