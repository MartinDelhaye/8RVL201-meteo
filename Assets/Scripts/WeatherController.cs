using UnityEngine;
using System;

public class WeatherController : MonoBehaviour
{
    public static WeatherController Instance;

    public int currentDayIndex = 0;
    public int currentHourIndex = 0;

    public int weatherCode;
    public bool isDay;
    private bool isInitialized = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (WeatherManager.Instance != null) Init();
        else WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        if(isInitialized) return;
        isInitialized = true;

        WeatherManager.OnWeatherReady -= Init;
        if (WeatherManager.Instance != null && WeatherManager.Instance.isDataRecuperee)
        {
            DisplayCurrent();
        }
    }

    public void DisplayCurrent()
    {
        var weatherData = WeatherManager.Instance.weatherData;

        WeatherDay day = weatherData.weatherDays[currentDayIndex];
        WeatherHour hour = day.weatherHours[currentHourIndex];

        weatherCode = hour.weatherCode;
        isDay = hour.IsDay();

        WeatherDisplay.Instance.UpdateDisplay(day, hour);
    }


    public void NextDay()
    {
        var weatherDays = WeatherManager.Instance.WeatherDays;

        if (currentDayIndex < weatherDays.Length - 1)
        {
            currentDayIndex++;
            currentHourIndex = 0;
            DisplayCurrent();
        }
    }

    public void PreviousDay()
    {
        if (currentDayIndex > 0)
        {
            currentDayIndex--;
            currentHourIndex = 0;
            DisplayCurrent();
        }
    }

    public void NextHour()
    {
        var weatherDays = WeatherManager.Instance.WeatherDays;

        if (currentHourIndex < weatherDays[currentDayIndex].weatherHours.Length - 1)
        {
            currentHourIndex++;
            DisplayCurrent();
        }
    }

    public void PreviousHour()
    {
        if (currentHourIndex > 0)
        {
            currentHourIndex--;
            DisplayCurrent();
        }
    }
}