using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ExpeditionMap
{
    public class ExpeditionManager : MonoBehaviour
    {
        public static ExpeditionManager Instance = null;

        public GameObject newRaportsButton = null;
        public TextMeshProUGUI numberOfFreeTeamsText = null;

        private void Awake()
        {
            if (ExpeditionManager.Instance == null)
                ExpeditionManager.Instance = this;
            else
                Destroy(this);
        }

        private List<Expedition> expeditions = new List<Expedition>();

        private int currentAvailableTeams = 1;

        private void Start()
        {
            currentAvailableTeams = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Rolnictwo].level;

            numberOfFreeTeamsText.text = currentAvailableTeams + "";

            RaportsReader.Instance.OnAllRaportsReaded += OnAllRaportsReaded;
            CategoriesProgressController.Instance.OnCategoryLevelUp += OnCategoryLevelUp;
        }

        public void CreateExpedition(Field destinationField)
        {
            if (currentAvailableTeams <= 0)
                return;

            expeditions.Add(new Expedition(destinationField));

            destinationField.SetAsExpeditionTarget(true);

            currentAvailableTeams--;

            numberOfFreeTeamsText.text = currentAvailableTeams + "";
        }

        public bool CanSendExpedition()
        {
            return currentAvailableTeams > 0;
        }

        List<RaportData> waitingRaports = new List<RaportData>();

        public void ExpeditionFinished(RaportData raportData)
        {
            waitingRaports.Add(raportData);

            newRaportsButton?.SetActive(true);
        }

        public void OnCategoryLevelUp(CategoriesProgressController.ScienceCategory category)
        {
            if (category == CategoriesProgressController.ScienceCategory.Rolnictwo)
            {
                AddFreeTeam();
            }
        }

        public void AddFreeTeam()
        {
            currentAvailableTeams++;

            numberOfFreeTeamsText.text = currentAvailableTeams + "";
        }

        public void OnAllRaportsReaded()
        {
            if (waitingRaports.Count <= 0)
                newRaportsButton?.SetActive(false);
        }

        #region Buttons

        public void ShowNewRaports()
        {
            RaportsReader.Instance.ShowRaports(waitingRaports.ToArray());

            waitingRaports.Clear();
        }

        #endregion
    }
}
