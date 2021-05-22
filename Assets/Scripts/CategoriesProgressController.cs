using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CategoriesProgressController : MonoBehaviour
{
    public Action<ScienceCategory> OnCategoryLevelUp;

    public static CategoriesProgressController Instance = null;

    [SerializeField] private int costIncreaseMultiplier = 1;
    public Science [] sciences;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        UpdateUI();

    }

    public void AddPointsToScienceHover(ScienceCategory scienceCategory, int value)
    {
        if (value == 0)
            return;

        sciences[(int)scienceCategory].progressText.text = sciences[(int)scienceCategory].currentExp + "+<color=yellow>" + value +"</color>/" + sciences[(int)scienceCategory].expToNextLevel;
        sciences[(int)scienceCategory].hoverProgressBar.fillAmount = (float)((sciences[(int)scienceCategory].currentExp + value) / (float)sciences[(int)scienceCategory].expToNextLevel);
    }

    public void StopHover()
    {
        //clean up all bars anyway
        for(int i = 0; i < 4; i++)
        {
            sciences[i].progressText.text = sciences[i].progressText.text = sciences[i].currentExp + "/" + sciences[i].expToNextLevel;
            sciences[i].hoverProgressBar.fillAmount = 0;
        }
    }

    public void AddPointsToScience(ScienceCategory scienceCategory, int value)
    {
        if(sciences[(int)scienceCategory].currentExp + value >= sciences[(int)scienceCategory].expToNextLevel)
        {
            int difference = (sciences[(int)scienceCategory].currentExp + value) - sciences[(int)scienceCategory].expToNextLevel;
            sciences[(int)scienceCategory].level++;
            sciences[(int)scienceCategory].currentExp = difference;
            sciences[(int)scienceCategory].expToNextLevel *= costIncreaseMultiplier;
            PopUpController.Instance.AddToList(scienceCategory);

            //if (scienceCategory == ScienceCategory.Rolnictwo)
            //    ExpeditionMap.ExpeditionManager.Instance?.AddFreeTeam();
            OnCategoryLevelUp?.Invoke(scienceCategory);
        }
        else
            sciences[(int)scienceCategory].currentExp += value;

        UpdateUI();
    }

    public void TestAddPointsToScience(int scienceCategory)
    {
        int value = 1;
        if (sciences[scienceCategory].currentExp + value >= sciences[(int)scienceCategory].expToNextLevel)
        {
            int difference = (sciences[scienceCategory].currentExp + value) - sciences[scienceCategory].expToNextLevel;
            sciences[scienceCategory].level++;
            sciences[scienceCategory].currentExp = difference;
            sciences[scienceCategory].expToNextLevel *= costIncreaseMultiplier;

        }
        else
            sciences[scienceCategory].currentExp += value;

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < sciences.Length; i++)
        {
            sciences[i].levelText.text = "Poziom " + sciences[i].level.ToString();
            sciences[i].progressText.text = sciences[i].currentExp + "/" + sciences[i].expToNextLevel;
            sciences[i].progressBar.fillAmount = (float)(sciences[i].currentExp / (float)sciences[i].expToNextLevel);
            sciences[i].hoverProgressBar.fillAmount = 0;
        }
    }

    // PLACEHOLDERS
    public enum ScienceCategory
    {
        Energetyka,
        Telekomunikacja,
        Transport,
        Społeczność,
    }

    [System.Serializable]
    public class Science
    {
        public ScienceCategory scienceCategory;
        public int level;
        public int expToNextLevel;
        public int currentExp;
        public Image progressBar;
        public Image hoverProgressBar;
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI levelText;

    }
}
