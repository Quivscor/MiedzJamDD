using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class ExpeditionMapManager : MonoBehaviour
    {
        [Header("References:")]
        public GameObject RootField = null;

        [SerializeField] private Vector2Int mapSize = Vector2Int.zero;

        public Vector2Int MapSize { get => mapSize; }

        private GameObject[,] fieldsMap = null;
        private GameObject activeField = null;

        private void Start()
        {
            fieldsMap = new GameObject[MapSize.x, MapSize.y];

            SetFieldsMap();
        }

        public void SetActiveField(int x, int z)
        {
            if (fieldsMap != null && x < mapSize.x && x >= 0 && z < mapSize.y && z >= 0)
            {
                activeField?.GetComponent<Animator>()?.SetBool("isActive", false);
                activeField = fieldsMap[x, z];
                activeField?.GetComponent<Animator>()?.SetBool("isActive", true);
            }
        }

        public void SetFieldsMap()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int x = (int)(i / MapSize.x);
                fieldsMap[x, i - (x * mapSize.x)] = this.transform.GetChild(i).gameObject;
            }
        }
    }
}
