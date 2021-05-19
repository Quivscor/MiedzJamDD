using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CategoriesProgressController : MonoBehaviour
{
    public static CategoriesProgressController Instance = null;

    [SerializeField] private float percentageCostIncrease = 0.5f;
    [SerializeField] private Science [] sciences;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        UpdateUI();

    }

    public void AddPointsToScience(ScienceCategory scienceCategory, int value)
    {
        if(sciences[(int)scienceCategory].currentExp + value >= sciences[(int)scienceCategory].expToNextLevel)
        {
            int difference = (sciences[(int)scienceCategory].currentExp + value) - sciences[(int)scienceCategory].expToNextLevel;
            sciences[(int)scienceCategory].level++;
            sciences[(int)scienceCategory].currentExp = difference;
            sciences[(int)scienceCategory].expToNextLevel += (int)(sciences[(int)scienceCategory].expToNextLevel * percentageCostIncrease);

        }
        else
            sciences[(int)scienceCategory].currentExp += value;

        UpdateUI();
    }

    public void TestAddPointsToScience(int scienceCategory)
    {
        int value = 3;
        if (sciences[scienceCategory].currentExp + value >= sciences[(int)scienceCategory].expToNextLevel)
        {
            int difference = (sciences[scienceCategory].currentExp + value) - sciences[scienceCategory].expToNextLevel;
            sciences[scienceCategory].level++;
            sciences[scienceCategory].currentExp = difference;
            sciences[scienceCategory].expToNextLevel += (int)(sciences[scienceCategory].expToNextLevel * percentageCostIncrease);

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
        }
    }

    // PLACEHOLDERS
    public enum ScienceCategory
    {
        Kategoria1,
        Kategoria2,
        Kategoria3,
        Kategoria4,
        Kategoria5
    }

    [System.Serializable]
    public class Science

    {
        public ScienceCategory scienceCategory;
        public int level;
        public int expToNextLevel;
        public int currentExp;
        public Image progressBar;
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI levelText;

    }
}
