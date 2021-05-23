using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class EscMenu : MonoBehaviour
{
    public GameObject HUD;
    public Slider slider;
    public AudioMixer mixer;

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

    public void OnSliderChange()
    {
        mixer.SetFloat("MyExposedParam", slider.value);
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
