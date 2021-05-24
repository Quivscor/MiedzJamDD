using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagesHolder : MonoBehaviour
{
    private static PackageData[] packages;

    private void Awake()
    {
        packages = new PackageData[4];

        packages[0] = Resources.Load<PackageData>("PackageEnergetyka");
        packages[1] = Resources.Load<PackageData>("PackageTelekomunikacja");
        packages[2] = Resources.Load<PackageData>("PackageTransport");
        packages[3] = Resources.Load<PackageData>("PackageSpołeczność");
    }

    public static CategoriesProgressController.ScienceCategory CheckCategoryFromID(string ID)
    {
        foreach (PackageData package in packages)
        {
            foreach(Building building in package.buildings)
            {
                if(building.m_buildingData.thisBuildingID == ID)
                {
                    return building.m_buildingData.pointCategory;
                }
            }
        }

        // default
        return CategoriesProgressController.ScienceCategory.Energetics;
    }
}
