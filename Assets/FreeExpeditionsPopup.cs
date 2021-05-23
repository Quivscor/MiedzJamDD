using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeExpeditionsPopup : MonoBehaviour
{
    public GameObject HUD;

    public void OnSwitchMaps()
    {
        if (ExpeditionMap.ExpeditionManager.Instance.GetCurrentAvailableTeams > 0)
            HUD.SetActive(true);
        else
            HUD.SetActive(false);
    }
}
