using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionButtons : MonoBehaviour
{
    public static InteractionButtons Instance;

    [Header("Buttons")]
    public GameObject nextDayButton;
    public GameObject previousDayButton;
    public GameObject nextHourButton;
    public GameObject previousHourButton;

    [Header("Colors")]
    public Color activeColor = Color.red;
    public Color disabledColor = Color.gray;
    public Color hoverColor = Color.white;

    // Renderers
    private Renderer nextDayRenderer;
    private Renderer previousDayRenderer;
    private Renderer nextHourRenderer;
    private Renderer previousHourRenderer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        nextDayRenderer = nextDayButton.GetComponent<Renderer>();
        previousDayRenderer = previousDayButton.GetComponent<Renderer>();
        nextHourRenderer = nextHourButton.GetComponent<Renderer>();
        previousHourRenderer = previousHourButton.GetComponent<Renderer>();

        UpdateButtonStates();
        WeatherManager.OnWeatherReady -= Init;
    }

    // Button click handlers
    public void NextDayButton()
    {
        WeatherController.Instance.NextDay();
        UpdateButtonStates();
    }

    public void PreviousDayButton()
    {
        WeatherController.Instance.PreviousDay();
        UpdateButtonStates();
    }

    public void NextHourButton()
    {
        WeatherController.Instance.NextHour();
        UpdateButtonStates();
    }

    public void PreviousHourButton()
    {
        WeatherController.Instance.PreviousHour();
        UpdateButtonStates();
    }

    // Hover enter / exit
    public void OnHoverEnter(GameObject button)
    {
        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = Color.white;
    }

    public void OnHoverExit(GameObject button)
    {
        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = Color.clear; // ou couleur par défaut
    }

    // Update button states based on current day/hour
    public void UpdateButtonStates()
    {
        if (WeatherManager.Instance == null) return;

        var weatherDays = WeatherManager.Instance.WeatherDays;
        int dayIndex = WeatherController.Instance.currentDayIndex;
        int hourIndex = WeatherController.Instance.currentHourIndex;

        bool canNextDay = dayIndex < weatherDays.Length - 1;
        bool canPrevDay = dayIndex > 0;
        bool canNextHour = !(dayIndex == weatherDays.Length - 1 &&
                             hourIndex == weatherDays[dayIndex].weatherHours.Length - 1);
        bool canPrevHour = !(dayIndex == 0 && hourIndex == 0);

        SetButtonState(nextDayButton, nextDayRenderer, canNextDay);
        SetButtonState(previousDayButton, previousDayRenderer, canPrevDay);
        SetButtonState(nextHourButton, nextHourRenderer, canNextHour);
        SetButtonState(previousHourButton, previousHourRenderer, canPrevHour);
    }

    private void SetButtonState(GameObject button, Renderer rend, bool enabled)
    {
        rend.material.color = enabled ? activeColor : disabledColor;
        Collider col = button.GetComponent<Collider>();
        if(col != null) col.enabled = enabled;
    }
}