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

    public void AddPointsToScience(int scienceCategory)
    {
        float value = 1;
        if(sciences[(int)scienceCategory].currentExp + value > sciences[(int)scienceCategory].expToNextLevel)
        {
            float difference = (sciences[(int)scienceCategory].currentExp + value) - sciences[(int)scienceCategory].expToNextLevel;
            sciences[(int)scienceCategory].level++;
            sciences[(int)scienceCategory].currentExp = difference;
            sciences[(int)scienceCategory].expToNextLevel += sciences[(int)scienceCategory].expToNextLevel * percentageCostIncrease;
            sciences[(int)scienceCategory].expToNextLevel = Mathf.Round(sciences[(int)scienceCategory].expToNextLevel);

        }

        sciences[(int)scienceCategory].currentExp += value;

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < sciences.Length; i++)
        {
            sciences[i].levelText.text = "Poziom " + sciences[i].level.ToString();
            sciences[i].progressText.text = sciences[i].currentExp + "/" + sciences[i].expToNextLevel;
            sciences[i].progressBar.fillAmount = sciences[i].currentExp / sciences[i].expToNextLevel;
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
        public float expToNextLevel;
        public float currentExp;
        public Image progressBar;
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI levelText;

    }
}
