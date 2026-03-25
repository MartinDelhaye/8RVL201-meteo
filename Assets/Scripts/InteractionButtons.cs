using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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
    public Color hoverColor = Color.pink;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;


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
    void PlayClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Button click handlers
    public void NextDayButton()
    {
        PlayClickSound();
        WeatherController.Instance.NextDay();
        UpdateButtonStates();
    }

    public void PreviousDayButton()
    {
        PlayClickSound();
        WeatherController.Instance.PreviousDay();
        UpdateButtonStates();
    }

    public void NextHourButton()
    {
        PlayClickSound();
        WeatherController.Instance.NextHour();
        UpdateButtonStates();
    }

    public void PreviousHourButton()
    {
        PlayClickSound();
        WeatherController.Instance.PreviousHour();
        UpdateButtonStates();
    }

    public void OnHoverEnter(XRBaseInteractable interactable)
    {
        if (interactable == null) return;
        Renderer rend = interactable.gameObject.GetComponent<Renderer>();
        rend.material.color = hoverColor;
    }

    public void OnHoverExit(XRBaseInteractable interactable)
    {
        if (interactable == null) return;
        Renderer rend = interactable.gameObject.GetComponent<Renderer>();
        rend.material.color = activeColor;
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
        if (col != null) col.enabled = enabled;
    }
}