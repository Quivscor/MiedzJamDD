﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeController : MonoBehaviour
{
    public Action OnEndOfTheWeek;
    public static TimeController Instance = null;

    [SerializeField] private TextMeshProUGUI daysText;
    [SerializeField] private TextMeshProUGUI monthsText;
    [SerializeField] private TextMeshProUGUI currentDayText;

    private DayType currentDay;
    private int totalDays = 1;
    private int thisMonthDays = 1;
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
        totalDays = 1;
        months = 0;
        currentDay = DayType.Niedziela;
        UpdateUI();
    }

    public void SkipToSunday()
    {
        while(currentDay != DayType.Niedziela)
        {
            NextDay();
        }
    }

    public void NextDay()
    {
        if (CurrentDay != DayType.Niedziela)
        {
            currentDay++;
            
            if (CurrentDay == DayType.Niedziela)
            {
                OnEndOfTheWeek?.Invoke();
            }
        }
        else
        {
            currentDay = DayType.Poniedziałek;

            // FAZA BUDOWANIA
        }

        totalDays++;
        thisMonthDays++;
        if (thisMonthDays % 30 == 0)
        {
            thisMonthDays = 0;
            months++;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if(currentDayText)
            currentDayText.text = currentDay.ToString();

        if(daysText)
            daysText.text = thisMonthDays.ToString();

        if(monthsText)
            monthsText.text = months.ToString();
    }

    // Polish signs in code ;_;
    public enum DayType
    {
        Poniedziałek,
        Wtorek,
        Środa,
        Czwartek,
        Piątek,
        Sobota,
        Niedziela
    }
}
