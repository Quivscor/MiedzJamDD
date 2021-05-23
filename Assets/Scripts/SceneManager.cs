using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance = null;

    public GameObject[] citySceneObjects;
    public GameObject[] expeditionSceneObjects;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

    }

    private void Start()
    {
        SwitchToExpeditionScene();
    }

    public void SwitchToCityScene()
    {
        HUDController.Instance.ToggleCityHUD(true);

        foreach (GameObject go in citySceneObjects)
            go.SetActive(true);

        foreach (GameObject go in expeditionSceneObjects)
            go.SetActive(false);
    }

    public void SwitchToExpeditionScene()
    {
        HUDController.Instance.ToggleCityHUD(false);


        foreach (GameObject go in citySceneObjects)
            go.SetActive(false);

        foreach (GameObject go in expeditionSceneObjects)
            go.SetActive(true);
    }
}
