using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpeditionMap
{
    public class ExpeditionManager : MonoBehaviour
    {
        public static ExpeditionManager Instance = null;

        private void Awake()
        {
            if (ExpeditionManager.Instance == null)
                ExpeditionManager.Instance = this;
            else
                Destroy(this);
        }

        [SerializeField] private int availableTeams = 2;

        private List<Expedition> expeditions = new List<Expedition>();

        private int currentAvailableTeams = 0;

        private void Start()
        {
            currentAvailableTeams = availableTeams;

            TimeController.Instance.OnEndOfTheWeek += FinishExpeditions;

            RaportsReader.Instance.OnAllRaportsReaded += OnRaportsReaded;
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

        public void FinishExpeditions()
        {
            List<RaportData> raportsData = new List<RaportData>();

            foreach (Expedition expedition in expeditions)
            {
                raportsData.Add(expedition.OnExpeditionFinished());
            }

            if (expeditions.Count > 0)
                RaportsReader.Instance.ShowRaports(raportsData.ToArray());
            else
                OnRaportsReaded();

            expeditions.Clear();
        }

        public void OnRaportsReaded()
        {
            currentAvailableTeams = availableTeams;
        }
    }
}
