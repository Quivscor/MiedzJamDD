﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageInventory : MonoBehaviour
{
    List<Building> buildings;

    private void Start()
    {
        CityDirector.Instance.OnBuildingPlaced += RemoveBuildingFromInventory;
    }

    public void FillInventoryFromPackage(PackageData package)
    {
        buildings.Clear();

        foreach(Building b in package.buildings)
        {
            buildings.Add(b);
        }
    }

    public void RemoveBuildingFromInventory(CityDirectorEventData eventData)
    {
        buildings.Remove(eventData.placedBuilding);
    }
}
