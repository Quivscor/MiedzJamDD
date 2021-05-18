using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FastMapGenerator : MonoBehaviour
{
    public ExpeditionMap.ExpeditionMapManager mapManager = null;
    public GameObject root = null;
    public GameObject fieldPrefab = null;
    public Vector2 mapDimentions = Vector2.zero;

    public void GenerateMap()
    {
        for (int x = 0; x < mapDimentions.x; x++)
        {
            for (int z = 0; z < mapDimentions.y; z++)
            {
                GameObject fieldGO = Instantiate(fieldPrefab, this.transform.position + new Vector3(x + 0.5f, 0, z + 0.5f), Quaternion.identity, root.transform);
                fieldGO.name = "Field (" + x + ", " + z + ")";
            }
        }
    }
}
