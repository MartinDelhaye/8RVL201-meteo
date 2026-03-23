using System;

[Serializable]
public class WeatherData
{
    public WeatherDay[] weatherDays;
    public bool isDataRecuperee = false;
}

[Serializable]
public class WeatherDay
{
    public DateTime date;
    public WeatherHour[] weatherHours;

    public float GetMinTemp()
    {
        float min = weatherHours[0].temperature;
        for (int i = 1; i < weatherHours.Length; i++)
        {
            if (weatherHours[i].temperature < min)
                min = weatherHours[i].temperature;
        }
        return min;
    }

    public float GetMaxTemp()
    {
        float max = weatherHours[0].temperature;
        for (int i = 1; i < weatherHours.Length; i++)
        {
            if (weatherHours[i].temperature > max)
                max = weatherHours[i].temperature;
        }
        return max;
    }
}


[Serializable]
public class WeatherHour
{
    public DateTime dateTime;
    public float temperature;
    public int weatherCode;
    public bool IsDay()
    {
        int hour = dateTime.Hour;
        return (hour >= 6 && hour < 18);
    }
    public int Hour => dateTime.Hour;

}