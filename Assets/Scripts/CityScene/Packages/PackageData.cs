using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CategoriesProgressController;

[System.Serializable, CreateAssetMenu(fileName = "PackageData", menuName = "Package Data")]
public class PackageData : ScriptableObject
{
    public ScienceCategory scienceCategory;
    public int cost;
    public List<Building> buildings;

    public Building GetRandomBuilding()
    {
        return buildings[Random.Range(0, buildings.Count)];
    }
}
