using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExpeditionMap
{
    public class Field : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private float maxCopper = 0.0f;
        [SerializeField] private bool hasBeenDiscovered = false;

        private float currentCopper = 0.0f;
        private int distanceToRoot = 0;
        private Vector2Int fieldCoords = Vector2Int.zero;

        private Animator animator = null;
        private FieldUI fieldUI = null;
        private FieldVisual fieldVisual = null;

        public float MaxCopper { get => maxCopper; }
        public int DistanceToRoot { get => distanceToRoot; }
        public bool IsRoot { get => isRoot; }

        private void Start()
        {
            currentCopper = maxCopper;

            animator = GetComponent<Animator>();
            fieldUI = GetComponent<FieldUI>();
            fieldVisual = GetComponent<FieldVisual>();

            fieldCoords.x = (int)this.transform.position.x;
            fieldCoords.y = (int)this.transform.position.z;

            fieldVisual?.SetFieldDiscovered(hasBeenDiscovered);
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
                if (hasBeenDiscovered)
                {
                    fieldData += "\nCopper: " + currentCopper + "/" + maxCopper;
                }
                else
                {
                    fieldData += "\nCopper: ???/???";
                }

                fieldData += "\nDist2Root: " + distanceToRoot;
            }

            return fieldData;
        }

        public void SetActive(bool value)
        {
            animator?.SetBool("isActive", value);

            if (value && !isSelected)
                fieldUI?.ShowFieldInfo(this);
            else
                fieldUI?.HideFieldInfo();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetActive(false);
        }

        private bool isSelected = false;

        public void OnPointerClick(PointerEventData eventData)
        {
            ExpeditionMapManager.Instance?.SetSelectedField(this);

            isSelected = true;
            fieldUI?.HideFieldInfo();
            fieldUI?.ShowExpeditionInfo(true);
        }

        public void OnDeselect()
        {
            isSelected = false;
            fieldUI?.ShowExpeditionInfo(false);
        }
    }
}
