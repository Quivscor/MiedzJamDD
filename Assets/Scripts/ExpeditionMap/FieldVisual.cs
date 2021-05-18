using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldVisual : MonoBehaviour
{
    public Material hiddenMaterial = null;

    private Material[] materialsForModels = null;
    private MeshRenderer[] modelsOnField = null;

    private void Awake()
    {
        modelsOnField = GetComponentsInChildren<MeshRenderer>();

        if (modelsOnField != null)
            materialsForModels = new Material[modelsOnField.Length];

        for (int i = 0; i < modelsOnField.Length; i++)
        {
            materialsForModels[i] = modelsOnField[i].material;
        }
    }

    public void SetFieldDiscovered(bool value)
    {
        for (int i = 0; i < modelsOnField.Length; i++)
        {
            modelsOnField[i].sharedMaterial = (value ? materialsForModels[i] : hiddenMaterial);
        }
    }
}
