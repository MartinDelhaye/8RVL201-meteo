using UnityEngine;
using System.Collections;

public class GestionFenetre : MonoBehaviour
{
    int lastCode = -1; // code météo par défaut

    [Header("Particles")]
    public GameObject rainParticles;
    public GameObject snowParticles;
    public ParticleSystem rainPS;
    public ParticleSystem snowPS;

    [Header("Skyboxes")]
    public Material skySun;
    public Material skyNight;
    public Material skyRain;
    public Material skyCloud;

    [Header("Render Textures")]
    public RenderTexture camEte;
    public RenderTexture camNeige;

    [Header("Objet à modifier")]
    public Renderer planeRenderer;

    void Start()
    {
        WeatherManager.OnWeatherReady += Init;
    }

    void Init()
    {
        WeatherManager.OnWeatherReady -= Init;
        UpdateMeteo();
    }

    void Update()
    {
        if (WeatherController.Instance == null) return;

        int currentCode = WeatherController.Instance.weatherCode;

        if (currentCode != lastCode)
        {
            lastCode = currentCode;
            UpdateMeteo();
        }
    }

    void UpdateMeteo()
    {
        int codeMeteo = WeatherController.Instance.weatherCode;
        bool isDay = WeatherController.Instance.isDay;

        bool isSun = (codeMeteo >= 0 && codeMeteo <= 19);
        bool isCloud = (codeMeteo >= 20 && codeMeteo <= 49);
        bool isSnow = (codeMeteo >= 70 && codeMeteo <= 79);
        bool isRain = (codeMeteo >= 50 && codeMeteo <= 69) || (codeMeteo >= 80 && codeMeteo <= 99);

        // 🔹 Active/Désactive les particules
        rainParticles.SetActive(isRain);
        snowParticles.SetActive(isSnow);

        if (isRain)
        {
            rainPS.Play();
            snowPS.Stop();
        }
        else if (isSnow)
        {
            rainPS.Stop();
            snowPS.Play();
        }
        else
        {
            rainPS.Stop();
            snowPS.Stop();
        }

        // 🔹 Change la skybox et la texture
        if (isSnow)
        {
            RenderSettings.skybox = skyRain; // ou skySnow si tu en as
            planeRenderer.material.SetTexture("_BaseMap", camNeige);
        }
        else if (isRain)
        {
            RenderSettings.skybox = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
        }
        else if (isCloud)
        {
            RenderSettings.skybox = skyCloud;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
        }
        else if (isSun)
        {
            RenderSettings.skybox = skySun;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
        }

        // 🔥 Mise à jour lumière globale
        DynamicGI.UpdateEnvironment();
    }
}