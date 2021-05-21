using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageDisplay : MonoBehaviour
{
    public MeshRenderer[] glowElements;
    public Image icon;
    public int index = 0;
    
    public void SetupPackage(PackageSettings settings)
    {
        foreach (MeshRenderer mr in glowElements)
        {
            mr.material = settings.glowMaterial;
        }

        icon.sprite = settings.icon;
        icon.color = settings.iconColor;

        GetComponent<Animator>()?.SetTrigger("openingTrigger");
    }

    public void OnSpawnBuildingWhileOpening()
    {
        PackageDisplayManager.Instance?.SpawnBuilding(index);
    }

    public void SetupPackageVisual(PackageSettings settings)
    {
        foreach (MeshRenderer mr in glowElements)
        {
            mr.material = settings.glowMaterial;
        }

        icon.sprite = settings.icon;
        icon.color = settings.iconColor;
    }
}
