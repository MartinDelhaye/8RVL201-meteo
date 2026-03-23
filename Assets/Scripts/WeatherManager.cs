using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public WeatherDay[] forecastDays;

    public bool isDataRecuperee = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(FetchWeather());
    }

    IEnumerator FetchWeather()
    {
        DateTime startDate = DateTime.Now;
        DateTime endDate = startDate.AddDays(6);

        string url = "https://api.open-meteo.com/v1/forecast?" +
                     "latitude=45.5&longitude=-73.6" +
                     "&hourly=temperature_2m,weathercode" +
                     "&start_date=" + startDate.ToString("yyyy-MM-dd") +
                     "&end_date=" + endDate.ToString("yyyy-MM-dd") +
                     "&timezone=auto";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            HourlyForecastData data = JsonUtility.FromJson<HourlyForecastData>(json);

            TransformData(data);
            isDataRecuperee = true;
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    void TransformData(HourlyForecastData data)
    {
        Dictionary<string, List<WeatherHour>> daysDict = new Dictionary<string, List<WeatherHour>>();

        int count = data.hourly.time.Length;

        for (int i = 0; i < count; i++)
        {
            DateTime dt = DateTime.Parse(data.hourly.time[i]);
            string dayKey = dt.ToString("yyyy-MM-dd");

            WeatherHour hour = new WeatherHour
            {
                dateTime = dt,
                temperature = data.hourly.temperature_2m[i],
                weatherCode = data.hourly.weathercode[i]
            };

            if (!daysDict.ContainsKey(dayKey))
                daysDict[dayKey] = new List<WeatherHour>();

            daysDict[dayKey].Add(hour);
        }

        forecastDays = new WeatherDay[daysDict.Count];

        int index = 0;
        foreach (var kvp in daysDict)
        {
            forecastDays[index] = new WeatherDay
            {
                date = DateTime.Parse(kvp.Key),
                hours = kvp.Value.ToArray()
            };
            index++;
        }
    }
}

// Class pour les données
[System.Serializable]
public class WeatherDay
{
    public DateTime date;
    public WeatherHour[] hours;
}

[System.Serializable]
public class WeatherHour
{
    public DateTime dateTime;
    public float temperature;
    public int weatherCode;

    public bool isDay => dateTime.Hour >= 6 && dateTime.Hour < 18;
}


// Class pour le JSON 
[System.Serializable]
public class HourlyForecastData
{
    public HourlyData hourly;
}

[System.Serializable]
public class HourlyData
{
    public string[] time;
    public float[] temperature_2m;
    public int[] weathercode;
}