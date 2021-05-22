using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsComboVisualManager
{
    private Building[] buildings;
    private int[] pointsToAdd;

    private int currentBuildingPlayingIndex = 0;

    public PointsComboVisualManager(Building[] buildings, int[] points)
    {
        this.buildings = buildings;
        this.pointsToAdd = points;

        currentBuildingPlayingIndex = 0;

        if (currentBuildingPlayingIndex < buildings.Length)
            buildings[currentBuildingPlayingIndex].ProcessPlacingInAnimation(points[currentBuildingPlayingIndex], this);
    }

    public void OnVisualAddingFinished()
    {
        currentBuildingPlayingIndex++;

        if (currentBuildingPlayingIndex < buildings.Length)
            buildings[currentBuildingPlayingIndex].ShowVisualAddedPoints(pointsToAdd[currentBuildingPlayingIndex], this);
    }
}
