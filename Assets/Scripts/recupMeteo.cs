using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class RecupMeteo : MonoBehaviour
{
    public TextMeshProUGUI temperatureText;

    void Start()
    {
        StartCoroutine(GetWeather());
    }

    IEnumerator GetWeather()
    {
        string url = "https://api.open-meteo.com/v1/forecast?latitude=45.5&longitude=-73.6&current_weather=true";

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            WeatherData data = JsonUtility.FromJson<WeatherData>(json);

            float temp = data.current_weather.temperature;

            temperatureText.text = temp.ToString("0.0") + "°C";
        }
        else
        {
            temperatureText.text = "Erreur API";
        }
    }
}

[System.Serializable]
public class WeatherData
{
    public CurrentWeather current_weather;
}

[System.Serializable]
public class CurrentWeather
{
    public float temperature;
}