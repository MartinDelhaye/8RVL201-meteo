using UnityEngine;
using TMPro;
using System;

public class WeatherDisplay : MonoBehaviour
{
    public static WeatherDisplay Instance;

    public TextMeshProUGUI jourNombre;
    public TextMeshProUGUI jourNom;
    public TextMeshProUGUI mois;
    public TextMeshProUGUI annee;
    public TextMeshProUGUI heureText;
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI tempMaxMinText;
    public TextMeshProUGUI loadingText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateDisplay(WeatherDay day, WeatherHour hour)
    {
        DateTime dateTime = hour.dateTime;

        jourNombre.text = dateTime.ToString("dd");
        jourNom.text = dateTime.ToString("dddd");
        mois.text = dateTime.ToString("MMMM");
        annee.text = dateTime.ToString("yyyy");

        int nextHour = (dateTime.Hour + 1) % 24;
        heureText.text = dateTime.Hour.ToString("00") + "h - " + nextHour.ToString("00") + "h";

        tempText.text = hour.temperature.ToString("0.0") + "°C";
        tempMaxMinText.text = "Max : " + day.GetMaxTemp().ToString("0.0") + "°C\nMin : " + day.GetMinTemp().ToString("0.0") + "°C";
    }

    public void DisplayError()
    {
        loadingText.text = "Erreur lors de la récupération des données météo.";
        jourNom.text = "";
        jourNombre.text = "";
        mois.text = "";
        annee.text = "";
        heureText.text = "";
        tempText.text = "";
        tempMaxMinText.text = "";
    }
}