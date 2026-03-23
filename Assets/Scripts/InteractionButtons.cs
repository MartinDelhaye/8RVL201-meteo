using UnityEngine;
using System.Collections;

public class InteractionButtons : MonoBehaviour
{
    public GameObject nextDayButton;
    public GameObject previousDayButton;

    public Color activeColor = Color.red;
    public Color disabledColor = Color.gray;

    private Renderer nextDayButtonRenderer;
    private Renderer previousDayButtonRenderer;
    private bool isInitialized = false;


    void Start()
    {
        if (WeatherManager.Instance != null) Init();
        else WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        if(isInitialized) return;
        isInitialized = true;
        nextDayButtonRenderer = nextDayButton.GetComponent<Renderer>();
        previousDayButtonRenderer = previousDayButton.GetComponent<Renderer>();

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

    private void UpdateButtonStates()
    {
        int currentIndex = WeatherController.Instance.currentDayIndex;
        int maxIndex = WeatherManager.Instance.WeatherDays.Length - 1;

        bool canNext = currentIndex < maxIndex;
        SetButtonState(nextDayButton, nextDayButtonRenderer, canNext);

        bool canPrev = currentIndex > 0;
        SetButtonState(previousDayButton, previousDayButtonRenderer, canPrev);
    }

    private void SetButtonState(GameObject button, Renderer buttonRenderer, bool enabled)
    {
        buttonRenderer.material.color = enabled ? activeColor : disabledColor;
        Collider col = button.GetComponent<Collider>();
        col.enabled = enabled;
    }
}