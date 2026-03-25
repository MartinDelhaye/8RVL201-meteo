using UnityEngine;
using System;

public class WeatherController : MonoBehaviour
{
    public static WeatherController Instance;

    public int currentDayIndex = 0;
    public int currentHourIndex = 0;

    public int weatherCode;
    public bool isDay;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        WeatherManager.OnWeatherReady -= Init;

        if (WeatherManager.Instance != null && WeatherManager.Instance.isDataRecuperee)
        {
            DisplayCurrent();
        }
    }

    public void DisplayCurrent()
    {
        if (WeatherManager.Instance == null)
        {
            Debug.LogWarning("WeatherManager.Instance is null !");
            return;
        }

        var weatherData = WeatherManager.Instance.weatherData;
        if (weatherData == null || weatherData.weatherDays == null || weatherData.weatherDays.Length == 0)
        {
            Debug.LogWarning("Pas de données météo disponibles !");
            return;
        }

        if (currentDayIndex >= weatherData.weatherDays.Length)
        {
            Debug.LogWarning("currentDayIndex est hors limites !");
            currentDayIndex = 0;
        }

        WeatherDay day = weatherData.weatherDays[currentDayIndex];

        if (day.weatherHours == null || day.weatherHours.Length == 0)
        {
            Debug.LogWarning("Pas d'heures disponibles pour ce jour !");
            return;
        }

        if (currentHourIndex >= day.weatherHours.Length)
            currentHourIndex = 0;

        WeatherHour hour = day.weatherHours[currentHourIndex];

        weatherCode = hour.weatherCode;
        isDay = hour.IsDay();

        if (WeatherDisplay.Instance != null)
            WeatherDisplay.Instance.UpdateDisplay(day, hour);
    }


    public void NextDay()
    {
        var weatherDays = WeatherManager.Instance.WeatherDays;

        if (currentDayIndex < weatherDays.Length - 1)
        {
            currentDayIndex++;
            DisplayCurrent();
        }
    }

    public void PreviousDay()
    {
        if (currentDayIndex > 0)
        {
            currentDayIndex--;
            DisplayCurrent();
        }
    }

    public void NextHour()
    {
        var weatherDays = WeatherManager.Instance.WeatherDays;

        if (currentHourIndex < weatherDays[currentDayIndex].weatherHours.Length - 1)
        {
            currentHourIndex++;
        }
        else if (currentDayIndex < weatherDays.Length - 1)
        {
            currentDayIndex++;
            currentHourIndex = 0;
        }
        DisplayCurrent();
    }

    public void PreviousHour()
    {
        if (currentHourIndex > 0)
        {
            currentHourIndex--;
        }
        else if (currentDayIndex > 0)
        {
            currentDayIndex--;
            var weatherDays = WeatherManager.Instance.WeatherDays;
            currentHourIndex = weatherDays[currentDayIndex].weatherHours.Length - 1;
        }
        DisplayCurrent();
    }
}