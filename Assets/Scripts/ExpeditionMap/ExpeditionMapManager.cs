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

        private void Start()
        {
            fieldsMap = new Field[MapSize.x, MapSize.y];

            SetFieldsMap();
            InitializeFields();
        }

        public void SetFieldsMap()
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                for (int z = 0; z < MapSize.y; z++)
                {
                    for (int i = 0; i < this.transform.childCount; i++)
                    {
                        Transform child = this.transform.GetChild(i);

                        if ((int)child.transform.position.x == x && (int)child.transform.position.z == z)
                        {
                            fieldsMap[x, z] = child.transform.GetComponent<Field>();
                            break;
                        }
                    }
                }
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
