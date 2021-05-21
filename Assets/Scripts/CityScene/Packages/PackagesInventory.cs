using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CategoriesProgressController;
using UnityEngine.UI;

public class PackagesInventory : MonoBehaviour
{
    [Header("References:")]
    public PackageData[] packageDatas = new PackageData[4];
    public GameObject mainShopCanvas = null;
    public Animator shopAnimator;
    public Animator scienceAnimator;
    public Animator buildDescriptionAnimator;
    public Button[] buyButtons = new Button[4];

    public Button openShopButton = null;
    public Button openPackageButton = null;


    [SerializeField] private int buildingsInOnePackage = 3;

    List<ScienceCategory> packages = new List<ScienceCategory>();

    private void Start()
    {

    }

    public void AddPackage(ScienceCategory category)
    {
        packages.Add(category);
    }

    public void OpenPackage()
    {
        if (packages.Count == 0 || BuildingsInventory.Instance.FreeSlots < buildingsInOnePackage || !PackageDisplayManager.Instance.CanOpenPack)
            return;

        ScienceCategory category = packages[0];

        packages.RemoveAt(0);

        Building[] buildingsToAdd = new Building[buildingsInOnePackage];

        for (int i = 0; i < buildingsToAdd.Length; i++)
        {
            buildingsToAdd[i] = GetRandomBuildingFromCategory(category);
        }

        BuildingsInventory.Instance?.AddBuildingsToInventory(buildingsToAdd, category);
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
        buildDescriptionAnimator.SetTrigger("HideBuildDescription");
        UpdateButtonsInteractions();
    }

    public void CloseShop()
    {
        //mainShopCanvas?.SetActive(false);
        shopAnimator.SetTrigger("HideShop");
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
}
