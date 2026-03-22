using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class RecupMeteo : MonoBehaviour
{
    public static RecupMeteo Instance;
    public TextMeshProUGUI temperatureText;

    public bool isDay = true;
    public int weatherCode = 0;
    public bool isDataRecuperee = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateWeather();
    }

    public void UpdateWeather()
    {
        StartCoroutine(GetWeather());
    }

    IEnumerator GetWeather()
    {
        DateTime date = GestionDate.Instance.GetCurrentDate();
        string formattedDate = date.ToString("yyyy-MM-dd");

        string url = "https://api.open-meteo.com/v1/forecast?latitude=45.5&longitude=-73.6"
            + "&daily=temperature_2m_max,temperature_2m_min,weathercode"
            + "&start_date=" + formattedDate
            + "&end_date=" + formattedDate
            + "&timezone=auto";
            

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            WeatherDailyData data = JsonUtility.FromJson<WeatherDailyData>(json);

            float tempMax = data.daily.temperature_2m_max[0];
            float tempMin = data.daily.temperature_2m_min[0];
            int code = data.daily.weathercode[0];

            temperatureText.text =
                tempMax.ToString("0") + "° / " +
                tempMin.ToString("0") + "°";

            weatherCode = code;
        }
        else
        {
            temperatureText.text = "Erreur API";
        }
    }
}

[System.Serializable]
public class WeatherDailyData
{
    public DailyData daily;
}

[System.Serializable]
public class DailyData
{
    public float[] temperature_2m_max;
    public float[] temperature_2m_min;
    public int[] weathercode;
}