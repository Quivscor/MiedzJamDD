﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CategoriesProgressController;
using UnityEngine.UI;

public class PackagesInventory : MonoBehaviour
{
    [Header("References:")]
    public PackageData[] packageDatas = new PackageData[4];
    public GameObject mainShopCanvas = null;
    public Animator scienceAnimator;
    public Animator buildDescriptionAnimator;
    public Button[] buyButtons = new Button[4];

    public Button openShopButton = null;
    public Button openPackageButton = null;


    [SerializeField] private int buildingsInOnePackage = 3;

    List<ScienceCategory> packages = new List<ScienceCategory>();

    private void Start()
    {
        TimeController.Instance.OnEndOfTheWeek += OnSunday;
        TimeController.Instance.OnFirstCommonDay += OnCommonDay;
    }

    public void AddPackage(ScienceCategory category)
    {
        packages.Add(category);
    }

    public void OpenPackage()
    {
        if (packages.Count == 0 || BuildingsInventory.Instance.FreeSlots < buildingsInOnePackage)
            return;

        ScienceCategory category = packages[0];

        packages.RemoveAt(0);

        Building[] buildingsToAdd = new Building[buildingsInOnePackage];

        for (int i = 0; i < buildingsToAdd.Length; i++)
        {
            buildingsToAdd[i] = GetRandomBuildingFromCategory(category);
        }

        BuildingsInventory.Instance?.AddBuildingsToInventory(buildingsToAdd);
    }

    private Building GetRandomBuildingFromCategory(ScienceCategory category)
    {
        PackageData packageData = null;

        for (int i = 0; i < packageDatas.Length; i++)
            if (packageDatas[i].scienceCategory == category)
            {
                packageData = packageDatas[i];
                break;
            }

        return packageData.GetRandomBuilding();
    }

    public void AddRandomPackage()
    {
        AddPackage((ScienceCategory)Random.Range(0, 4));
    }

    #region Button Actions

    public void OpenShop()
    {
        mainShopCanvas?.SetActive(true);
        scienceAnimator.SetTrigger("HideScience");
        buildDescriptionAnimator.SetTrigger("HideBuildDescription");
        UpdateButtonsInteractions();
    }

    public void CloseShop()
    {
        mainShopCanvas?.SetActive(false);
        scienceAnimator.SetTrigger("ShowScience");
        buildDescriptionAnimator.SetTrigger("ShowBuildDescription");
    }

    public void BuyRolnictwoPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Rolnictwo);
    }

    public void BuyEnergetykaPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Energetyka);
    }

    public void BuyTelekomunikacjaPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Telekomunikacja);
    }

    public void BuyTransportPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Transport);
    }

    #endregion

    private void UpdateButtonsInteractions()
    {
        bool canBuy = ResourceController.Instance.Copper >= 100;

        foreach (Button b in buyButtons)
            b.interactable = canBuy;
    }

    private void OnSunday()
    {
        if (mainShopCanvas.gameObject.activeSelf == true)
            CloseShop();

        openShopButton.interactable = false;
        openPackageButton.interactable = false;
    }

    private void OnCommonDay()
    {
        openShopButton.interactable = true;
        openPackageButton.interactable = true;
    }

}
