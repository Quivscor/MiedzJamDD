using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "PackageData", menuName = "Package Data")]
public class PackageData : ScriptableObject
{
    public int cost;
    public List<Building> buildings;
}
