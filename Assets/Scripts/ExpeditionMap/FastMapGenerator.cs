using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FastMapGenerator : MonoBehaviour
{
    public GameObject root = null;
    public GameObject fieldPrefab = null;
    public Vector2 mapDimentions = Vector2.zero;

    public bool execute = false;

    private void Update()
    {
        if (!execute)
            return;

        execute = !execute;

        for (int x = 0; x < mapDimentions.x; x++)
        {
            for (int z = 0; z < mapDimentions.y; z++)
            {
                GameObject fieldGO = Instantiate(fieldPrefab, this.transform.position + new Vector3(x, 0, z), Quaternion.identity, root.transform);
                fieldGO.name = "Field (" + x + ", " + z + ")";
            }
        }
    }
}
