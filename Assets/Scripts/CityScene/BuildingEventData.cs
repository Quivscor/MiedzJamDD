using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEventData
{
    public string buildingName;
    public string category;
    public string description;
    public string bonuses;

    public BuildingEventData(string buildingName, string category, string description, string bonuses)
    {
        this.buildingName = buildingName;
        this.category = category;
        this.description = description;
        this.bonuses = bonuses;
    }

}
