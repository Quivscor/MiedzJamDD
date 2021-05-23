using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

namespace ExpeditionMap
{
    public class ExpeditionManager : MonoBehaviour
    {
        public static ExpeditionManager Instance = null;

        public GameObject newRaportsButton = null;
        public GameObject newRaportsInCitySceneButton = null;
        public TextMeshProUGUI numberOfFreeTeamsText = null;
        public Action<int> OnExpeditionConfirmation;
        public Action<int> OnReceiveRaport;
        private Animator popupAnimator;

        public AudioClip[] placeExpedition;
        public AudioSource audioSource;

        private FreeExpeditionsPopup newPopup;

        private void Awake()
        {
            if (ExpeditionManager.Instance == null)
                ExpeditionManager.Instance = this;
            else
                Destroy(this);

            audioSource = GetComponent<AudioSource>();
            popupAnimator = newRaportsInCitySceneButton.GetComponent<Animator>();
            newPopup = FindObjectOfType<FreeExpeditionsPopup>();
        }

        private List<Expedition> expeditions = new List<Expedition>();

        private int currentAvailableTeams = 1;
        public int GetCurrentAvailableTeams => currentAvailableTeams;

        private void Start()
        {
            currentAvailableTeams = (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Społeczność].level;

            numberOfFreeTeamsText.text = currentAvailableTeams + "";

            RaportsReader.Instance.OnAllRaportsReaded += OnAllRaportsReaded;
            CategoriesProgressController.Instance.OnCategoryLevelUp += OnCategoryLevelUp;
        }

        public void CreateExpedition(Field destinationField)
        {
            if (currentAvailableTeams <= 0)
                return;

            OnExpeditionConfirmation?.Invoke(4); //On sending expedition tutorial

            Expedition expedition = new Expedition(destinationField);

            expeditions.Add(expedition);

            destinationField.SetAsExpeditionTarget(true);
            destinationField.SetExpedition(expedition);

            currentAvailableTeams--;

            CameraControllerExp.Instance?.Shake();
            audioSource.PlayOneShot(placeExpedition[Random.Range(0, placeExpedition.Length)]);

            newPopup.OnSwitchMaps(); //no map switch, but hides on no more available expeditions

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

            OnReceiveRaport?.Invoke(6); // On receiving raport tutorial

            newRaportsButton?.SetActive(true);
            newRaportsInCitySceneButton?.SetActive(true);
            popupAnimator.SetTrigger("ShowPopup");
        }

        public void OnCategoryLevelUp(CategoriesProgressController.ScienceCategory category)
        {
            if (category == CategoriesProgressController.ScienceCategory.Społeczność)
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
            {
                newRaportsButton?.SetActive(false);
                newRaportsInCitySceneButton?.SetActive(false);
            }
        }

        public bool CanSendExpedition(Field field)
        {
            return field.DistanceToRoot <= (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Energetyka].level * TeamStatsModifiers.DistanceModifier && field.HasBeenDiscovered && field.CurrentCopper > 0;
        }

        #region Buttons

        public void ShowNewRaports()
        {
            newRaportsButton?.SetActive(false);
            newRaportsInCitySceneButton?.SetActive(false);

            RaportsReader.Instance.ShowRaports(waitingRaports.ToArray());

            waitingRaports.Clear();
        }

        #endregion
    }
}
