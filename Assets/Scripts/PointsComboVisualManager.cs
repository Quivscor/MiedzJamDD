using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsComboVisualManager
{
    private Building[] buildings;
    private int[] pointsToAdd;

    private int currentBuildingPlayingIndex = 0;
    private AudioSource audioSource;
    public PointsComboVisualManager(Building[] buildings, int[] points, AudioSource audioSource)
    {
        this.buildings = buildings;
        this.pointsToAdd = points;
        this.audioSource = audioSource;

        currentBuildingPlayingIndex = 0;
        //audioSource.PlayOneShot(CityDirector.Instance.buildingSounds[Random.Range(0, CityDirector.Instance.buildingSounds.Length)]);

        if (currentBuildingPlayingIndex < buildings.Length)
            buildings[currentBuildingPlayingIndex].ProcessPlacingInAnimation(points[currentBuildingPlayingIndex], this);
    }

    public void OnVisualAddingFinished()
    {
        currentBuildingPlayingIndex++;

        if (currentBuildingPlayingIndex < buildings.Length)
        {
            buildings[currentBuildingPlayingIndex].ShowVisualAddedPoints(pointsToAdd[currentBuildingPlayingIndex], this);

            audioSource.PlayOneShot(CityDirector.Instance.combigSounds[Random.Range(0, CityDirector.Instance.combigSounds.Length)]);
        }

    }
}
