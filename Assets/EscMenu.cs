using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    public GameObject HUD;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (HUD.activeInHierarchy)
                HideMenu();
            else
                DisplayMenu();
        }
    }

    public void DisplayMenu()
    {
        HUD.SetActive(true);
    }

    public void HideMenu()
    {
        HUD.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
