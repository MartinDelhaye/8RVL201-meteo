using UnityEngine;
using TMPro;
using System;

public class DateDisplay : MonoBehaviour
{
    public TextMeshProUGUI jourNombre;
    public TextMeshProUGUI jourNom;
    public TextMeshProUGUI mois;
    public TextMeshProUGUI annee;

    void Start()
    {
        DateTime now = DateTime.Now;

        jourNombre.text = now.ToString("dd");
        jourNom.text = now.ToString("dddd");
        mois.text = now.ToString("MMMM");
        annee.text = now.ToString("yyyy");
    }
}