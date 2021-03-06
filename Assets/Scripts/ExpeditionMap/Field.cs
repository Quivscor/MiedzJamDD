using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExpeditionMap
{
    public class Field : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("References:")]
        public GameObject expeditionModel = null;
        public GameObject normalModel = null;
        public GameObject exploredModel = null;
        public Material expiredMaterial = null;
        public GameObject border = null;
        public ParticleSystem placingParticles = null;

        public Material goodFieldMaterial = null;
        public Material badFieldMaterial = null;

        [Header("Paramteres:")]
        [SerializeField] private float maxCopper = 0.0f;
        [SerializeField] private bool hasBeenDiscovered = false;

        private float currentCopper = 0.0f;
        private int distanceToRoot = 0;
        private Vector2Int fieldCoords = Vector2Int.zero;

        private Animator animator = null;
        private FieldUI fieldUI = null;
        private FieldVisual fieldVisual = null;

        private Outline outline = null;

        public float MaxCopper { get => maxCopper; }
        public int DistanceToRoot { get => distanceToRoot; }
        public bool IsRoot { get => isRoot; }
        public Vector2Int FieldCoords { get => fieldCoords; }
        public float CurrentCopper 
        { 
            get => currentCopper;
            set
            {
                currentCopper = value;

                if (currentCopper <= 0)
                {
                    Debug.Log("Set material");
                    exploredModel.GetComponent<MeshRenderer>().material = expiredMaterial;
                }
            }
        }

        public bool HasBeenDiscovered 
        { 
            get => hasBeenDiscovered; 
            set
            {
                hasBeenDiscovered = value;
                fieldVisual?.SetFieldDiscovered(hasBeenDiscovered);
            }
        }

        public Animator Animator { get => animator; }

        private Expedition expedition = null;

        private void Start()
        {
            currentCopper = maxCopper;

            animator = GetComponent<Animator>();
            fieldUI = GetComponent<FieldUI>();
            fieldVisual = GetComponent<FieldVisual>();

            fieldCoords.x = (int)this.transform.position.x;
            fieldCoords.y = (int)this.transform.position.z;

            fieldVisual?.SetFieldDiscovered(hasBeenDiscovered);

            outline = GetComponentInChildren<Outline>();
        }

        private bool isRoot = false;
        private bool isSelected = false;
        private bool isExpeditionTarget = false;

        public void InitializeField(Vector3 root, bool isRoot)
        {
            this.isRoot = isRoot;

            fieldCoords.x = (int)this.transform.position.x;
            fieldCoords.y = (int)this.transform.position.z;

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

            maxCopper = 100 + (DistanceToRoot * 100) + (Random.Range(0, 3) * (50 * (int)(DistanceToRoot / 2)));
            currentCopper = maxCopper;
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
            /*if (outline)
            {
                outline.enabled = true;
                if (ExpeditionManager.Instance.CanSendExpedition(this))
                    outline.OutlineColor = Color.green;
                else
                    outline.OutlineColor = Color.red;
            }*/

            if (border)
            {
                border.SetActive(true);
                if (ExpeditionManager.Instance.CanSendExpedition(this))
                    border.GetComponent<MeshRenderer>().material = goodFieldMaterial;
                else
                    border.GetComponent<MeshRenderer>().material = badFieldMaterial;
            }

            //if (isExpeditionTarget)
            //    return;

            //SetActive(true);
            if (isExpeditionTarget && expedition != null)
                FieldInfoDisplay.Instance.DisplayFieldInfo(FieldCoords, (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Transport].level * TeamStatsModifiers.LoadModifier, (int)maxCopper, expedition.LeftTimeToFinishExpedition, isExpeditionTarget, DistanceToRoot);
            else
                FieldInfoDisplay.Instance.DisplayFieldInfo(FieldCoords, (int)currentCopper, (int)maxCopper, DistanceToRoot + 2, isExpeditionTarget, DistanceToRoot);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            border?.SetActive(false);
            //if (outline)
            //    outline.enabled = false;
            //SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isRoot || isExpeditionTarget || !ExpeditionManager.Instance.CanSendExpedition() || !(currentCopper > 0))
                return;

            if (DistanceToRoot > (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Energetyka].level * TeamStatsModifiers.DistanceModifier)
            {
                Debug.Log("Cannot reach field with given distance!");
                return;
            }

            ExpeditionMapManager.Instance?.SetSelectedField(this);

            isSelected = true;
            fieldUI?.HideFieldInfo();
            //fieldUI?.ShowExpeditionInfo(true);
            ExpeditionPopUp.Instance.SetupFieldInfo(this, (int)currentCopper, DistanceToRoot + 2);
        }

        public void OnDeselect()
        {
            isSelected = false;
            fieldUI?.ShowExpeditionInfo(false);
        }

        public void SetAsExpeditionTarget(bool value)
        {
            isExpeditionTarget = value;

            //expeditionModel?.SetActive(value);

            if (!value)
            {
                normalModel?.SetActive(false);
                exploredModel?.SetActive(true);
                //outline.enabled = false;
                //outline = exploredModel?.GetComponent<Outline>();
                //outline.enabled = false;

                exploredModel.transform.GetChild(0)?.gameObject.SetActive(false);
            }
            else
            {
                placingParticles?.Play();

                normalModel?.SetActive(false);
                exploredModel?.SetActive(true);
                //outline.enabled = false;
                //outline = exploredModel?.GetComponent<Outline>();
                //outline.enabled = false;

                exploredModel.transform.GetChild(0)?.gameObject.SetActive(true);
            }
        }

        public void SetExpedition(Expedition expedition)
        {
            this.expedition = expedition;

            animator?.SetTrigger("pressedTrigger");
        }

        #region Anim Events

        public void OnReachImpact1()
        {
            //ExpeditionMapManager.Instance?.OnExpeditionImpact1(this);
        }

        public void OnReachImpact2()
        {
            //ExpeditionMapManager.Instance?.OnExpeditionImpact2(this);
        }

        #endregion

        #region Button Actions

        public void CreateExpeditionToThisField()
        {
            ExpeditionManager.Instance?.CreateExpedition(this);

            ExpeditionMapManager.Instance?.SetSelectedField(null);
        }

        #endregion
    }
}
