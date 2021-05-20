using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] citySceneObjects;
    public GameObject[] expeditionSceneObjects;

    public void SwitchToCityScene()
    {
        foreach (GameObject go in citySceneObjects)
            go.SetActive(true);

        foreach (GameObject go in expeditionSceneObjects)
            go.SetActive(false);
    }

    public void SwitchToExpeditionScene()
    {
        foreach (GameObject go in citySceneObjects)
            go.SetActive(false);

        foreach (GameObject go in expeditionSceneObjects)
            go.SetActive(true);
    }
}
