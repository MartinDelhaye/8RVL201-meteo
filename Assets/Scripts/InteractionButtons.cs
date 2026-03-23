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

    IEnumerator Start()
    {
        while (WeatherManager.Instance == null || !WeatherManager.Instance.isDataRecuperee)
        {
            yield return null;
        }

        nextDayButtonRenderer = nextDayButton.GetComponent<Renderer>();
        previousDayButtonRenderer = previousDayButton.GetComponent<Renderer>();

        UpdateButtonStates();
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
        int maxIndex = WeatherManager.Instance.forecastDays.Length - 1;

        bool canNext = currentIndex < maxIndex;
        SetButtonState(nextDayButton, nextDayButtonRenderer, canNext);

        bool canPrev = currentIndex > 0;
        SetButtonState(previousDayButton, previousDayButtonRenderer, canPrev);
    }

    private void SetButtonState(GameObject button, Renderer rend, bool enabled)
    {
        rend.material.color = enabled ? activeColor : disabledColor;
        Collider col = button.GetComponent<Collider>();
        col.enabled = enabled;
    }
}