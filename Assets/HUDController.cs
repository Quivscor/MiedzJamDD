using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance = null;

    [SerializeField] GameObject daysObject;
    [SerializeField] GameObject buttonsObject;
    [SerializeField] GameObject copperObject;
    [SerializeField] GameObject scienceObject;
    [SerializeField] GameObject buildingDescriptionObject;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void ToggleCityHUD(bool toggle)
    {
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
