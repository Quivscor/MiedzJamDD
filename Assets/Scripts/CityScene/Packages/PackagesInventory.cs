using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CategoriesProgressController;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class PackagesInventory : MonoBehaviour
{
    [Header("References:")]
    public PackageData[] packageDatas = new PackageData[4];
    public GameObject mainShopCanvas = null;
    public Animator shopAnimator;
    public Animator scienceAnimator;
    public Animator buildDescriptionAnimator;
    public Animator buildInventoryAnimator;
    public Button[] buyButtons = new Button[4];

    public Button openShopButton = null;
    public Button openPackageButton = null;

    public Action<int> OnBuyPackage;

    public PackageSettings[] packagesSettings = new PackageSettings[4];
    public PackageDisplay[] packageDisplay = new PackageDisplay[4];

    private Animator animator = null;


    [SerializeField] private int buildingsInOnePackage = 3;

    List<ScienceCategory> packages = new List<ScienceCategory>();

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AddPackage(ScienceCategory category)
    {
        packages.Add(category);

        if (packages.Count <= 4 && packages.Count != 0)
        {
            packageDisplay[packages.Count - 1].gameObject.SetActive(true);
            packageDisplay[packages.Count - 1].SetupPackageVisual(packagesSettings[(int)category]);
        }
    }

    private bool openingLocked = false;

    public void OpenPackage()
    {
        if (packages.Count == 0 || BuildingsInventory.Instance.FreeSlots < buildingsInOnePackage || !PackageDisplayManager.Instance.CanOpenPack || openingLocked)
            return;

        openingLocked = true;

        animator?.SetTrigger("openPackageTrigger");

        /*ScienceCategory category = packages[0];

        packages.RemoveAt(0);

        Building[] buildingsToAdd = new Building[buildingsInOnePackage];

        for (int i = 0; i < buildingsToAdd.Length; i++)
        {
            buildingsToAdd[i] = GetRandomBuildingFromCategory(category);
        }

        BuildingsInventory.Instance?.AddBuildingsToInventory(buildingsToAdd, category);*/
    }

    public void OpenPackageFromAnimation()
    {
        ScienceCategory category = packages[0];

        Building[] buildingsToAdd = new Building[buildingsInOnePackage];

        for (int i = 0; i < buildingsToAdd.Length; i++)
        {
            Building droppedBuilding = GetRandomBuildingFromCategory(category);

            for (int j = 0; j < i; j++)
                if (buildingsToAdd[j].BuildingCategory == droppedBuilding.BuildingCategory)
                    droppedBuilding = GetRandomBuildingFromCategory(category);

            buildingsToAdd[i] = droppedBuilding;
        }

        BuildingsInventory.Instance?.AddBuildingsToInventory(buildingsToAdd, category);

        openingLocked = false;

        packages.RemoveAt(0);
    }

    //call in animations
    public void UpdatePackagesColors()
    {
        OpenPackageFromAnimation();

        for (int i = 0; i < 4; i++)
            packageDisplay[i].gameObject.SetActive(false);

        for (int i = 0; i < (int)Mathf.Min(packages.Count, 4); i++)
        {
            packageDisplay[i].gameObject.SetActive(true);

            packageDisplay[i].SetupPackageVisual(packagesSettings[(int)packages[i]]);
        }
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
        //mainShopCanvas?.SetActive(true);
        shopAnimator.SetTrigger("ShowShop");
        scienceAnimator.SetTrigger("HideScience");
        buildInventoryAnimator?.SetTrigger("hideTrigger");
        buildDescriptionAnimator.SetTrigger("HideBuildDescription");
        UpdateButtonsInteractions();
    }

    public void CloseShop()
    {
        //mainShopCanvas?.SetActive(false);
        shopAnimator.SetTrigger("HideShop");
        scienceAnimator.SetTrigger("ShowScience");
        buildInventoryAnimator?.SetTrigger("showTrigger");
        buildDescriptionAnimator.SetTrigger("ShowBuildDescription");
    }

    public void BuyRolnictwoPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        OnBuyPackage?.Invoke(5); //Buying package tutorial
        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Społeczność);
    }

    public void BuyEnergetykaPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        OnBuyPackage?.Invoke(5); //Buying package tutorial
        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Energetyka);
    }

    public void BuyTelekomunikacjaPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        OnBuyPackage?.Invoke(5); //Buying package tutorial
        UpdateButtonsInteractions();

        AddPackage(ScienceCategory.Telekomunikacja);
    }

    public void BuyTransportPackage()
    {
        ResourceController.Instance?.SpendCopper(100);

        OnBuyPackage?.Invoke(5); //Buying package tutorial
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
}
