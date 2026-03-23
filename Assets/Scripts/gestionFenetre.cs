using UnityEngine;
using System.Collections;

public class GestionFenetre  : MonoBehaviour
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
        StartCoroutine(WaitForMeteo());
    }

    IEnumerator WaitForMeteo()
    {while (!RecupMeteo.Instance.isDataRecuperee)
        {yield return null;}
        UpdateMeteo();
    }

    void Update()
    {
        if (!RecupMeteo.Instance.isDataRecuperee) return;
        int currentCode = RecupMeteo.Instance.weatherCode;
        if (currentCode != lastCode) // 11 est un code météo par défaut
        {
            lastCode = currentCode;
            UpdateMeteo();
        }
    }

    void UpdateMeteo()
    {
        int codeMeteo = 11;
        bool isDay = RecupMeteo.Instance.isDay;

        bool isSun = (codeMeteo >= 0 && codeMeteo <= 19);
        bool isCloud = (codeMeteo >= 20 && codeMeteo <= 49);
        bool isSnow = (codeMeteo >= 70 && codeMeteo <= 79);
        bool isRain = (codeMeteo >= 50 && codeMeteo <= 69) || (codeMeteo >= 80 && codeMeteo <= 99);
        rainParticles.SetActive(true);
        snowParticles.SetActive(true);

        if (isSnow)
        {
            RenderSettings.skybox = skyRain;

            planeRenderer.material.SetTexture("_BaseMap", camNeige);
            rainPS.Stop();
            snowPS.Play();
        }
        else if (isRain)
        {
            RenderSettings.skybox = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
            snowPS.Stop();
            rainPS.Play();
        }
        else if (isCloud)
        {
            RenderSettings.skybox = skyCloud;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
            rainPS.Stop();
            snowPS.Stop();
        }
        else if (isSun)
        {
            RenderSettings.skybox = skySun;
            planeRenderer.material.SetTexture("_BaseMap", camEte);
            rainPS.Stop();
            snowPS.Stop();
        }

        // 🔥 important pour la lumière
        DynamicGI.UpdateEnvironment();
    }
}