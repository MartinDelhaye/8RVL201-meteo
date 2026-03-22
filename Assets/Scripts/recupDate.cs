using UnityEngine;
using TMPro;
using System;

public class RecupDate : MonoBehaviour
{
    public static RecupDate Instance;
    public TextMeshProUGUI jourNombre;
    public TextMeshProUGUI jourNom;
    public TextMeshProUGUI mois;
    public TextMeshProUGUI annee;
    private DateTime currentDate;

    void Awake()
    {
      Instance = this;  
    }

    void Start()
    {
        currentDate = DateTime.Now;
        UpdateDate();
    }

    public void AddDay(int value)
    {
        currentDate = currentDate.AddDays(value);
        UpdateDate();
    }

    void UpdateDate()
    {
        jourNombre.text = currentDate.ToString("dd");
        jourNom.text = currentDate.ToString("dddd");
        mois.text = currentDate.ToString("MMMM");
        annee.text = currentDate.ToString("yyyy");
    }
}