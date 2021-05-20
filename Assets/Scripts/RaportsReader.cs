using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaportsReader : MonoBehaviour
{
    public Action OnAllRaportsReaded;

    public static RaportsReader Instance = null;

    private void Awake()
    {
        if (RaportsReader.Instance == null)
            RaportsReader.Instance = this;
        else
            Destroy(this);
    }

    public GameObject raportPrefab = null;

    private RaportData[] raportsData = null;

    private int currentRaportIndex = 0;

    public void ShowRaports(RaportData[] raportsData)
    {
        this.raportsData = raportsData;

        currentRaportIndex = 0;

        GameObject raport = Instantiate(raportPrefab, this.transform.position, Quaternion.identity, this.transform);
        raport.GetComponent<Raport>().SetRaportData(raportsData[currentRaportIndex]);
    }

    public void RaportReaded()
    {
        currentRaportIndex++;

        if (currentRaportIndex < raportsData.Length)
        {
            GameObject raport = Instantiate(raportPrefab, this.transform.position, Quaternion.identity, this.transform);
            raport.GetComponent<Raport>().SetRaportData(raportsData[currentRaportIndex]);
        }
        else
        {
            OnAllRaportsReaded?.Invoke();
        }
    }
}
