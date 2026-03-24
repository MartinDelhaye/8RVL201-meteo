using UnityEngine;
using TMPro;
using System;


public class Horloge : MonoBehaviour
{
    public TextMeshProUGUI heureText;

    void Update()
    {
        DateTime currentTime = DateTime.Now;
        string formattedTime = currentTime.ToString("dd-MM-yyyy | HH:mm:ss");
        heureText.text = formattedTime;
    }
}