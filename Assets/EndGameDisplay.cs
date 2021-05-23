using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class EndGameDisplay : MonoBehaviour
{
    public GameObject entireHUD;

    public TMProText timeText;
    public TMProText copperText;
    public TMProText progressText;
    public TMProText rankingText;

    private int startYear;

    private void Start()
    {
        startYear = TimeController.Instance.Years;
        //DisplayGameEnd();
    }

    public void DisplayGameEnd()
    {
        entireHUD.SetActive(true);
        int years = TimeController.Instance.Years - startYear;
        string lata = "lata";
        if (years > 4)
            lata = "lat";
        int month = (int)TimeController.Instance.CurrentMonth;
        string miesiace = "miesiąc";
        if (month > 1)
            miesiace = "miesiące";
        if (month > 4)
            miesiace = "miesięcy";

        timeText.text = "Czas rozgrywki: " + years + " " + lata + " i " + " " + month + " " + miesiace;
        copperText.text = "Wydobyta miedź: " + ResourceController.Instance.LifetimeCopper;
        progressText.text = "Postępy w kategoriach\n<color=#FFFA34>Energetyka</color>\t\t\t" + CategoriesProgressController.Instance.sciences[0].level +
            "\n<color=#B900F8>Telekomunikacja</color>\t\t" + CategoriesProgressController.Instance.sciences[1].level +
            "\n<color=#CD1725>Transport</color>\t\t\t" + CategoriesProgressController.Instance.sciences[2].level +
            "\n<color=#00B917>Społeczeństwo</color>\t\t" + CategoriesProgressController.Instance.sciences[3].level;

        rankingText.text = "Otrzymujesz rangę: " + GenerateRanking();
    }

    private string GenerateRanking()
    {
        return "Mateusz Zawisza";
    }
}
