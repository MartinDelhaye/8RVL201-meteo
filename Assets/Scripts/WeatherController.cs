using UnityEngine;

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
        InvokeRepeating(nameof(TryInit), 0f, 0.5f);
    }

    void TryInit()
    {
        if (WeatherManager.Instance != null && WeatherManager.Instance.isDataRecuperee)
        {
            CancelInvoke(nameof(TryInit));
            DisplayCurrent();
        }
    }

    public void DisplayCurrent()
    {
        var days = WeatherManager.Instance.forecastDays;

        if (days == null) return;

        WeatherDay day = days[currentDayIndex];
        WeatherHour hour = day.hours[currentHourIndex];

        weatherCode = hour.weatherCode;
        isDay = hour.isDay;

        WeatherDisplay.Instance.UpdateDisplay(day, hour);
    }


    public void NextDay()
    {
        var days = WeatherManager.Instance.forecastDays;

        if (currentDayIndex < days.Length - 1)
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
        var day = WeatherManager.Instance.forecastDays[currentDayIndex];

        if (currentHourIndex < day.hours.Length - 1)
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