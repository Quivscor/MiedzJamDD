using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class ExpeditionManager : MonoBehaviour
    {
        public static ExpeditionManager Instance = null;

        public GameObject newRaportsButton = null;

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
            currentAvailableTeams = 1;

            RaportsReader.Instance.OnAllRaportsReaded += OnAllRaportsReaded;
        }

        public void CreateExpedition(Field destinationField)
        {
            if (currentAvailableTeams <= 0)
                return;

            expeditions.Add(new Expedition(destinationField));

            destinationField.SetAsExpeditionTarget(true);

            currentAvailableTeams--;
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

        public void AddFreeTeam()
        {
            currentAvailableTeams++;
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
