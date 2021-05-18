using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionMapData : MonoBehaviour
{
    public static ExpeditionMapData Instance = null;

    private void Awake()
    {
        if (ExpeditionMapData.Instance == null)
            ExpeditionMapData.Instance = this;
        else
            Destroy(this);
    }

    private FieldData[,] fieldData = null;

    public void CreateContainer(int x, int z)
    {
        if (fieldData == null)
            fieldData = new FieldData[x, z];
    }

    public void ClearData()
    {
        fieldData = null;
    }

    public void AddData(FieldData data, int x, int z)
    {
        fieldData[x, z] = data;
    }

    public FieldData GetData(int x, int z)
    {
        return fieldData[x, z];
    }
}
