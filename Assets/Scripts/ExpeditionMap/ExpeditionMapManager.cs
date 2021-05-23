using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class ExpeditionMapManager : MonoBehaviour
    {
        public static ExpeditionMapManager Instance = null;

        private void Awake()
        {
            if (ExpeditionMapManager.Instance == null)
                ExpeditionMapManager.Instance = this;
            else
                Destroy(this);
        }

        [SerializeField] private Vector2Int rootPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int mapSize = Vector2Int.zero;

        public Vector2Int MapSize { get => mapSize; }

        private Field[,] fieldsMap = null;
        private Field selectedField = null;

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

        public void SetSelectedField(Field field)
        {
            selectedField?.OnDeselect();
            selectedField = field;
        }

        public bool HasSelectedField()
        {
            return selectedField != null;
        }

        public Field[] GetAllNeighbours(Field field)
        {
            List<Field> neighbours = new List<Field>();

            for (int x = field.FieldCoords.x - 1; x < field.FieldCoords.x + 2; x++)
            {
                for (int z = field.FieldCoords.y - 1; z < field.FieldCoords.y + 2; z++)
                {
                    if (x >= 0 && x < MapSize.x && z >= 0 && z < MapSize.y && (x != field.FieldCoords.x || z != field.FieldCoords.y))
                    {
                        neighbours.Add(fieldsMap[x, z]);
                    }
                }
            }

            return neighbours.ToArray();
        }

        public void OnExpeditionImpact1(Field field)
        {
            int firstX = field.FieldCoords.x - 1;
            int lastX = field.FieldCoords.x + 2;
            int firstY = field.FieldCoords.y - 1;
            int lastY = field.FieldCoords.y + 2;

            for (int i = firstX; i < lastX; i++)
            {
                for (int j = firstY; j < lastY; j++)
                {
                    if (i == firstX || i == lastX - 1 || j == firstY || j == lastY - 1)
                    {
                        if (i > 0 && j > 0 && i < MapSize.x && j < MapSize.y && fieldsMap[i,j] != null)
                        {
                            fieldsMap[i, j]?.Animator?.SetTrigger("impact1Trigger");
                        }
                    }
                }
            }
        }

        public void OnExpeditionImpact2(Field field)
        {
            int firstX = field.FieldCoords.x - 2;
            int lastX = field.FieldCoords.x + 3;
            int firstY = field.FieldCoords.y - 2;
            int lastY = field.FieldCoords.y + 3;

            for (int i = firstX; i < lastX; i++)
            {
                for (int j = firstY; j < lastY; j++)
                {
                    if (i == firstX || i == lastX - 1|| j == firstY || j == lastY - 1)
                    {
                        if (i > 0 && j > 0 && i < MapSize.x && j < MapSize.y && fieldsMap[i, j] != null)
                        {
                            fieldsMap[i, j]?.Animator?.SetTrigger("impact2Trigger");
                        }
                    }
                }
            }
        }
    }
}
