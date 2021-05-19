using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDisplayerComponent : MonoBehaviour
{
    private List<BuildingField> displayedFields;
    private List<BuildingField> fieldsToCleanup;

    public Material defaultBuildingFieldMaterial;
    public Material boostedField;

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
    }

    public void HighlightBuildingFields()
    {
        foreach(BuildingField f in displayedFields)
        {
            f.Renderer.material = boostedField;
        }
    }

    //this is called after TryDisplayComboPotential
    public void CleanupDisplay(BuildingFieldEventData eventData)
    {
        foreach (BuildingField f in displayedFields)
        {
            f.Renderer.material = defaultBuildingFieldMaterial;
        }
    }
}
