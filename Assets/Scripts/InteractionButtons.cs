using UnityEngine;
using System.Collections;

public class InteractionButtons : MonoBehaviour
{
    public static InteractionButtons Instance;
    public GameObject nextDayButton;
    public GameObject previousDayButton;
    public GameObject nextHourButton;
    public GameObject previousHourButton;

    public Color activeColor = Color.red;
    public Color disabledColor = Color.gray;

    private Renderer nextDayButtonRenderer;
    private Renderer previousDayButtonRenderer;
    private Renderer nextHourButtonRenderer;
    private Renderer previousHourButtonRenderer;


    void Start()
    {
        WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        nextDayButtonRenderer = nextDayButton.GetComponent<Renderer>();
        previousDayButtonRenderer = previousDayButton.GetComponent<Renderer>();
        nextHourButtonRenderer = nextHourButton.GetComponent<Renderer>();
        previousHourButtonRenderer = previousHourButton.GetComponent<Renderer>();

        UpdateButtonStates();

        WeatherManager.OnWeatherReady -= Init;
    }

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

    public void UpdateButtonStates()
    {
        var weatherDays = WeatherManager.Instance.WeatherDays;
        int dayIndex = WeatherController.Instance.currentDayIndex;
        int hourIndex = WeatherController.Instance.currentHourIndex;

        bool canNextDay = dayIndex < weatherDays.Length - 1;
        bool canPrevDay = dayIndex > 0;

        SetButtonState(nextDayButton, nextDayButtonRenderer, canNextDay);
        SetButtonState(previousDayButton, previousDayButtonRenderer, canPrevDay);

        bool canNextHour = !(dayIndex == weatherDays.Length - 1 &&
                             hourIndex == weatherDays[dayIndex].weatherHours.Length - 1);
        bool canPrevHour = !(dayIndex == 0 && hourIndex == 0);

        SetButtonState(nextHourButton, nextHourButtonRenderer, canNextHour);
        SetButtonState(previousHourButton, previousHourButtonRenderer, canPrevHour);
    }

    private void SetButtonState(GameObject button, Renderer buttonRenderer, bool enabled)
    {
        buttonRenderer.material.color = enabled ? activeColor : disabledColor;
        Collider col = button.GetComponent<Collider>();
        col.enabled = enabled;
    }
}