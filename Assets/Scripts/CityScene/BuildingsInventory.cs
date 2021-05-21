using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingsInventory : MonoBehaviour
{
    public static BuildingsInventory Instance = null;

    [SerializeField] private Vector3 inventoryFirstSlot = Vector3.zero;
    [SerializeField] private Vector3 slotsOffset = Vector3.zero;
    [SerializeField] private int maxSlots = 6;

    public Action<int> OnEmptyInventory;

    public int FreeSlots { get; private set; }

    private void Awake()
    {
        if (BuildingsInventory.Instance == null)
            BuildingsInventory.Instance = this;
        else
            Destroy(this);

        FreeSlots = maxSlots;
    }

    private List<Building> buildings = new List<Building>();

    private void Start()
    {
        CityDirector.Instance.OnBuildingPlaced += RemoveBuildingFromInventory;
    }

    public void AddBuildingsToInventory(Building[] buildings)
    {
        foreach (Building b in buildings)
        {
            int slotNumber = this.buildings.Count;

            Vector3 slotPosition = inventoryFirstSlot + (slotsOffset * slotNumber);

            GameObject building = Instantiate(b.gameObject, slotPosition, Quaternion.identity);

            Building bScr = building.GetComponent<Building>();

            BuildingDescriptionController.Instance.AddToEvent(bScr);
            this.buildings.Add(bScr);
        }

        FreeSlots = maxSlots - this.buildings.Count;
    }

    public void RemoveBuildingFromInventory(CityDirectorEventData eventData)
    {
        buildings.Remove(eventData.placedBuilding);

        FreeSlots = maxSlots - this.buildings.Count;
        if (buildings.Count == 0)
            OnEmptyInventory?.Invoke(1); //this plays tutorial sequence 1

        RecalculateBuildingsInSlots();
    }

    private void RecalculateBuildingsInSlots()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].transform.position = inventoryFirstSlot + (slotsOffset * i);
        }
    }
}
