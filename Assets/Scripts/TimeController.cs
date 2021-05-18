using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance = null;

    private DayType currentDay;
    private int totalDays = 0;
    private int months = 0;

    public int TotalDays { get => totalDays; }
    public int Months { get => months; }
    public DayType CurrentDay { get => currentDay; }

    private void Awake()
    {
        if(!Instance)
            Instance = this;
    }
    
    void Start()
    {
        totalDays = 0;
        months = 0;
        currentDay = DayType.Sunday;
    }

    public void NextDay()
    {
        if (CurrentDay != DayType.Sunday)
        {
            currentDay ++;
            
            // FAZA EKSPEDYCJI 
        }
        else
        {
            currentDay = DayType.Monday;

            // FAZA BUDOWANIA
        }

        totalDays ++;
        if (totalDays % 30 == 0)
            months++;
    }

    public enum DayType
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}
