using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance = null;

    [Header("Animators")]
    [SerializeField] private Animator earthMissionsAnimator;
    [SerializeField] private Animator scienceAnimator;
    [SerializeField] private Animator buildDescriptionAnimator;
    [SerializeField] private Animator buildingsInventoryAnimator;

    [Header("Animators")]
    [SerializeField] GameObject daysObject;
    [SerializeField] GameObject buttonsObject;
    [SerializeField] GameObject copperObject;
    [SerializeField] GameObject scienceObject;
    [SerializeField] GameObject buildingDescriptionObject;

    private FreeExpeditionsPopup freeExpeditionsPopup;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        freeExpeditionsPopup = FindObjectOfType<FreeExpeditionsPopup>();
    }

    public void ShowEarthMissions()
    {
        scienceAnimator.SetTrigger("HideScience");
        buildDescriptionAnimator.SetTrigger("HideBuildDescription");
        earthMissionsAnimator.SetTrigger("ShowEarthMissions");
        buildingsInventoryAnimator?.SetTrigger("hideTrigger");
    }

    public void HideEarthMissions()
    {
        scienceAnimator.SetTrigger("ShowScience");
        buildDescriptionAnimator.SetTrigger("ShowBuildDescription");
        earthMissionsAnimator.SetTrigger("HideEarthMissions");
        buildingsInventoryAnimator?.SetTrigger("showTrigger");
    }

    public void ToggleCityHUD(bool toggle)
    {
        freeExpeditionsPopup.OnSwitchMaps();
        buttonsObject.SetActive(toggle);
        scienceObject.SetActive(toggle);
        buildingDescriptionObject.SetActive(toggle);
    }

    public void ToggleAllObjects(bool toggle)
    {
        buttonsObject.SetActive(toggle);
        scienceObject.SetActive(toggle);
        buildingDescriptionObject.SetActive(toggle);

        daysObject.SetActive(toggle);
        copperObject.SetActive(toggle);
    }
}
