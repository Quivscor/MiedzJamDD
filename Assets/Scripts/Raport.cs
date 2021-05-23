using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raport : MonoBehaviour
{
    [Header("References:")]
    public TextMeshProUGUI fieldCoordinatesText = null;
    public TextMeshProUGUI contextText = null;
    public TextMeshProUGUI gainedCopperText = null;
    public TextMeshProUGUI discoveredFieldsText = null;
    public TextMeshProUGUI additionalActivityText = null;

    public void SetRaportData(RaportData raportData)
    {
        this.fieldCoordinatesText.text = raportData.fieldCoordinatesText;
        this.contextText.text = raportData.contextText;
        this.gainedCopperText.text = raportData.gainedCopperText;
        this.discoveredFieldsText.text = raportData.discoveredFieldsText;

        this.additionalActivityText.gameObject.SetActive(raportData.additionalActivity);
        this.additionalActivityText.text = raportData.additionalActivityText;

        this.fieldCoordinatesText.text = raportData.fieldCoordinatesText;
    }

    public void RaportClosed()
    {
        RaportsReader.Instance?.RaportReaded();

        Destroy(this.gameObject);
    }
}
