using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "BuildingData", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string thisBuildingID;
    public string description;
    public List<string> buildingIDs;
    public List<int> buildingBoosts;

    public int pointValue;
    public CategoriesProgressController.ScienceCategory pointCategory;

    //coordinates relative to the owners position on city grid
    public List<Vector2Int> neighborCoordinatesRelative = new List<Vector2Int>();

    public Dictionary<string, int> GetNeighborBoostsData()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        for(int i = 0; i < buildingIDs.Count; i++)
        {
            result.Add(buildingIDs[i], buildingBoosts[i]);
        }
        return result;
    }
}
