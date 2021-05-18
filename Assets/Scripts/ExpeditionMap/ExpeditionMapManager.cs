using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class ExpeditionMapManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int rootPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int mapSize = Vector2Int.zero;

        public Vector2Int MapSize { get => mapSize; }

        private Field[,] fieldsMap = null;
        private Field activeField = null;

        private void Start()
        {
            fieldsMap = new Field[MapSize.x, MapSize.y];

            SetFieldsMap();
            InitializeFields();
        }

        public void SetActiveField(int x, int z)
        {
            activeField?.SetActive(false);

            if (fieldsMap != null && x < mapSize.x && x >= 0 && z < mapSize.y && z >= 0)
            {
                activeField = fieldsMap[x, z];
                activeField?.SetActive(true);
            }
        }

        public void SetFieldsMap()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int x = (int)(i / MapSize.x);

                Field field = this.transform.GetChild(i).gameObject.GetComponent<Field>();
                fieldsMap[x, i - (x * mapSize.x)] = field;
            }
        }

        public void InitializeFields()
        {
            Field rootField = fieldsMap[rootPosition.x, rootPosition.y];

            for (int x = 0; x < MapSize.x; x++)
            {
                for (int z = 0; z < MapSize.y; z++)
                {
                    fieldsMap[x, z].InitializeField(rootField.transform.position, fieldsMap[x,z] == rootField);
                }
            }
        }
    }
}
