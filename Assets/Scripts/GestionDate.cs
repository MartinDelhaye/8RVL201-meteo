using UnityEngine;
using TMPro;
using System;

public class GestionDate : MonoBehaviour
{
    public static GestionDate Instance;
    public TextMeshProUGUI jourNombre;
    public TextMeshProUGUI jourNom;
    public TextMeshProUGUI mois;
    public TextMeshProUGUI annee;
    public TextMeshProUGUI heureText;
    private DateTime currentDate;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentDate = DateTime.Now;
        currentDate = currentDate.AddSeconds(Time.deltaTime);
        UpdateDate();
    }

    public void AddDay(int value)
    {
        currentDate = currentDate.AddDays(value);
        UpdateDate();
        RecupMeteo.Instance.UpdateWeather();
    }

    void Update()
    {
        currentDate = currentDate.AddSeconds(Time.deltaTime);
        UpdateDate();
    }

    void UpdateDate()
    {
        jourNombre.text = currentDate.ToString("dd");
        jourNom.text = currentDate.ToString("dddd");
        mois.text = currentDate.ToString("MMMM");
        annee.text = currentDate.ToString("yyyy");
        heureText.text = currentDate.ToString("HH:mm:ss");
    }

    public DateTime GetCurrentDate()
    {
        return currentDate;
    }
}