using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsInventory : MonoBehaviour
{
    public static BuildingsInventory Instance = null;

    [SerializeField] private Vector3 inventoryFirstSlot = Vector3.zero;
    [SerializeField] private Vector3 slotsOffset = Vector3.zero;
    [SerializeField] private int maxSlots = 6;

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

        TimeController.Instance.OnEndOfTheWeek += OnSunday;
        TimeController.Instance.OnFirstCommonDay += OnCommonDay;
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

        RecalculateBuildingsInSlots();
    }

    private void RecalculateBuildingsInSlots()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].transform.position = inventoryFirstSlot + (slotsOffset * i);
        }
    }

    public void OnSunday()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].gameObject.SetActive(false);
        }
    }

    public void OnCommonDay()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].gameObject.SetActive(false);
        }
    }
}
