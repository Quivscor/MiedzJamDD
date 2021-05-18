using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private float maxCopper = 0.0f;

        private float currentCopper = 0.0f;
        private int distanceToRoot = 0;
        private Vector2Int fieldCoords = Vector2Int.zero;

        private Animator animator = null;
        private FieldUI fieldUI = null;

        public float MaxCopper { get => maxCopper; }
        public int DistanceToRoot { get => distanceToRoot; }
        public bool IsRoot { get => isRoot; }

        private void Start()
        {
            currentCopper = maxCopper;

            animator = GetComponent<Animator>();
            fieldUI = GetComponent<FieldUI>();

            fieldCoords.x = (int)this.transform.position.x;
            fieldCoords.y = (int)this.transform.position.z;
        }

        private bool isRoot = false;

        public void InitializeField(Vector3 root, bool isRoot)
        {
            this.isRoot = isRoot;
            CalculateDistanceToRoot(new Vector2Int((int)root.x, (int)root.z));
        }

        private void CalculateDistanceToRoot(Vector2Int rootPosition)
        {
            if (isRoot)
                return;

            int xDiff = (int)Mathf.Abs(rootPosition.x - fieldCoords.x);
            int zDiff = (int)Mathf.Abs(rootPosition.y - fieldCoords.y);

            distanceToRoot = xDiff > zDiff ? xDiff : zDiff;

            if (xDiff == zDiff)
                distanceToRoot++;
        }

        public override string ToString()
        {
            string fieldData = "";

            fieldData += (isRoot ? "Colony" : "Field");
            fieldData += "\n[" + fieldCoords.x + ", " + fieldCoords.y + "]";

            if (!isRoot)
            {
                fieldData += "\nCopper: " + currentCopper + "/" + maxCopper;
                fieldData += "\nDist2Root: " + distanceToRoot;
            }

            return fieldData;
        }

        public void SetActive(bool value)
        {
            animator?.SetBool("isActive", value);

            if (value)
                fieldUI?.ShowFieldInfo(this);
            else
                fieldUI?.HideFieldInfo();
        }
    }
}
