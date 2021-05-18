using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance = null;

    private int copper = 0;

    public int Copper { get => copper; }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void AddCopper(int value)
    {
        copper += value;
    }

    public void SpendCopper(int value)
    {
        copper -= value;

        if (copper < 0)
            Debug.LogError("COPPPER IS LESS THAN 0. CHECK IT AMOUNT BEFORE SPENDING");
    }

    public bool HasEnoughCopper(int value)
    {
        if (copper - value >= 0)
            return true;
        else
        {
            Debug.Log("You don't have enough copper.");
            return false;
        }
    }

}
