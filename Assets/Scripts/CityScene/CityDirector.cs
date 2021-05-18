using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityDirector : MonoBehaviour
{
    public static CityDirector Instance;

    private static int CityGridSize = 8;
    
    private readonly Building[,] m_cityGrid = new Building[CityGridSize, CityGridSize];
    public Building[,] CityGrid { get => m_cityGrid; }

    #region Setup
    [Header("Scene setup")]
    public GameObject buildingFieldPrefab;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        for (int x = 0; x < CityGridSize; x++)
        {
            for (int z = 0; z < CityGridSize; z++)
            {
                GameObject fieldGO = Instantiate(buildingFieldPrefab, this.transform.position + new Vector3(x + 0.5f, 0, z + 0.5f), Quaternion.identity, this.transform);
                fieldGO.name = "Field (" + x + ", " + z + ")";
            }
        }
    }

    private void Start()
    {
        
    }
}
