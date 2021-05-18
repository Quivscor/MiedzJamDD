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
        }

        public void CreateExpedition(Field destinationField)
        {
            if (currentAvailableTeams <= 0)
                return;

            expeditions.Add(new Expedition(destinationField));

            currentAvailableTeams--;
        }
    }
}
