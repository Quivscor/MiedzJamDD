using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeController : MonoBehaviour
{
    public Action OnEndOfTheWeek;
    public Action OnFirstCommonDay;
    public Action<int> OnEndOfDay;
    public Action OnNextDay;
    public static TimeController Instance = null;

    [SerializeField] private TextMeshProUGUI daysText;
    [SerializeField] private TextMeshProUGUI monthsText;
    [SerializeField] private TextMeshProUGUI currentDayText;

    private MonthType currentMonth;
    private int totalMonths = 1;
    private int thisYearMonths = 1;
    private int years = 0;

    public int TotalMonths { get => totalMonths; }
    public int Years { get => years; }
    public MonthType CurrentMonth { get => currentMonth; }

    private void Awake()
    {
        if(!Instance)
            Instance = this;
    }
    
    void Start()
    {
        totalMonths = 0;
        years = 0;
        currentMonth = MonthType.Styczeń;
        CityDirector.Instance.OnBuildingPlaced += NextDay;
        UpdateUI();
    }

    public void NextDay()
    {
        NextDay(null);
    }

    public void NextDay(CityDirectorEventData cityDirectorEventData)
    {
        if (CurrentMonth != MonthType.Grudzień)
        {
            currentMonth++;
            
        }
        else
        {
            currentMonth = MonthType.Styczeń;
            years++;
            //OnFirstCommonDay?.Invoke();
        }

        OnNextDay?.Invoke();

        totalMonths++;

        OnEndOfDay((int)currentMonth);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(currentDayText)
            currentDayText.text = currentMonth.ToString();

        //if(daysText)
        //    daysText.text = thisYearMonths.ToString();

        if(monthsText)
            monthsText.text = years.ToString();
    }

    // Polish signs in code ;_;
    public enum MonthType
    {
        Styczeń,
        Luty,
        Marzec,
        Kwiecień,
        Maj,
        Czerwiec,
        Lipiec,
        Sierpień,
        Wrzesień,
        Październik,
        Listopad,
        Grudzień
    }

}
