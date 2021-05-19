﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "BuildingData", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string thisBuildingID;

    public List<string> buildingIDs;
    public List<float> buildingBoosts;

    //coordinates relative to the owners position on city grid
    public List<Vector2Int> neighborCoordinatesRelative = new List<Vector2Int>();

    public Dictionary<string, float> GetNeighborBoostsData()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        for(int i = 0; i < buildingIDs.Count; i++)
        {
            result.Add(buildingIDs[i], buildingBoosts[i]);
        }
        return result;
    }
}