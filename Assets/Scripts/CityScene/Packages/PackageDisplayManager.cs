using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDisplayManager : MonoBehaviour
{
    public PackageSettings[] packagesSettings;

    public PackageDisplay[] packageDisplay;

    private Building[] buildingsToInstantiate = new Building[6];

    public static PackageDisplayManager Instance = null;

    private void Awake()
    {
        if (PackageDisplayManager.Instance == null)
            PackageDisplayManager.Instance = this;
        else
            Destroy(this);
    }

    public bool CanOpenPack { get; private set; } = true;

    private int currentFirstSlot = 0;

    public void ShowPackages(Building[] buildingsToInstantiate, int firstSlot, int numberOfSlots, CategoriesProgressController.ScienceCategory category)
    {
        CanOpenPack = false;

        int buildingToAddIndex = 0;

        for (int i = firstSlot; i < firstSlot + numberOfSlots; i++)
        {
            this.buildingsToInstantiate[i] = buildingsToInstantiate[buildingToAddIndex];
            packageDisplay[i].SetupPackage(packagesSettings[(int)category]);

            buildingToAddIndex++;
        }
    }

    public void SpawnBuilding(int index)
    {
        BuildingsInventory.Instance.SpawnBuilding(this.buildingsToInstantiate[index], index);

        this.buildingsToInstantiate[index] = null;

        bool readyToAnotherOpening = true;

        for (int i = 0; i < buildingsToInstantiate.Length; i++)
        {
            if (buildingsToInstantiate[i] != null)
                readyToAnotherOpening = false;
        }

        CanOpenPack = readyToAnotherOpening;
    }
}

[System.Serializable]
public struct PackageSettings
{
    public CategoriesProgressController.ScienceCategory category;
    public Material glowMaterial;
    public Sprite icon;
    public Color iconColor;
}