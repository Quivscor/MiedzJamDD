using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExpeditionMap;

public class FieldInfoDisplay : MonoBehaviour
{
    public static FieldInfoDisplay Instance = null;

    private void Awake()
    {
        if (FieldInfoDisplay.Instance == null)
            FieldInfoDisplay.Instance = this;
        else
            Destroy(this);
    }

    public TextMeshProUGUI fieldTitleText = null;
    public GameObject defaultInfo = null;
    public GameObject expeditionInfo = null;

    public TextMeshProUGUI copperLeftText = null;
    public TextMeshProUGUI timeToExtractText = null;

    public TextMeshProUGUI expectedCopperText = null;
    public TextMeshProUGUI expectedTimeText = null;

    public TextMeshProUGUI scienceChanceText = null;
    public TextMeshProUGUI distanceText = null;

    public void DisplayFieldInfo(Vector2Int fieldCoords, int copperLeft, int maxCopper, int timeToExtract, bool hasExpedition)
    {
        fieldTitleText.text = "Pozycja: [ " + fieldCoords.x + " , " + fieldCoords.y + " ]";

        if (hasExpedition)
        {
            defaultInfo.SetActive(false);
            expeditionInfo.SetActive(true);

            expectedCopperText.text = copperLeft + "";

            expectedTimeText.text = timeToExtract + " " + (timeToExtract != 1 ? "dni" : "dzień");
        }
        else
        {
            defaultInfo.SetActive(true);
            expeditionInfo.SetActive(false);

            copperLeftText.text = copperLeft + " / " + maxCopper;
            timeToExtractText.text = timeToExtract + " dni";
        }

        distanceText.text = (timeToExtract - 2) + "";

        //scienceChanceText.text = scienceChance + "";
    }
}
