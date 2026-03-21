using UnityEngine;
using TMPro;
using System;

public class recupDate : MonoBehaviour
{
    public TextMeshProUGUI dateText;

    void Start()
    {
        DateTime now = DateTime.Now;
        dateText.text = now.ToString("dd MMMM yyyy");
    }
}
