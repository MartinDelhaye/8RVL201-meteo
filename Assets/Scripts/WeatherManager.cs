using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public WeatherData weatherData;
    public WeatherDay[] WeatherDays => weatherData?.weatherDays;

    public bool isDataRecuperee => weatherData != null && weatherData.isDataRecuperee;
    public static event Action OnWeatherReady;

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
            weatherData.isDataRecuperee = true;

            int actualHour = DateTime.Now.Hour;
            WeatherController.Instance.currentHourIndex = actualHour;
            OnWeatherReady?.Invoke();
        }
        else
        {
            WeatherDisplay.Instance.DisplayError();
            weatherData = new WeatherData
            {
                weatherDays = new WeatherDay[0],
                isDataRecuperee = false
            };
            OnWeatherReady?.Invoke();
        }
    }

    void TransformData(HourlyForecastData data)
    {
        Dictionary<string, List<WeatherHour>> dataDict = new Dictionary<string, List<WeatherHour>>();

        int countData = data.hourly.time.Length;

        for (int i = 0; i < countData; i++)
        {
            DateTime dateTime = DateTime.Parse(data.hourly.time[i]);
            string dayKey = dateTime.ToString("yyyy-MM-dd");
            if (!dataDict.ContainsKey(dayKey)) dataDict[dayKey] = new List<WeatherHour>();

            WeatherHour hour = new WeatherHour
            {
                dateTime = dateTime,
                temperature = data.hourly.temperature_2m[i],
                weatherCode = data.hourly.weathercode[i]
            };
            dataDict[dayKey].Add(hour);
        }

        weatherData = new WeatherData();
        weatherData.weatherDays = new WeatherDay[dataDict.Count];

        int index = 0;

        foreach (var focus in dataDict)
        {
            weatherData.weatherDays[index] = new WeatherDay
            {
                date = DateTime.Parse(focus.Key),
                weatherHours = focus.Value.ToArray()
            };

            index++;
        }
    }
}


// Class pour le JSON 
[Serializable]
public class HourlyForecastData
{
    public HourlyData hourly;
}

[Serializable]
public class HourlyData
{
    public string[] time;
    public float[] temperature_2m;
    public int[] weathercode;
}