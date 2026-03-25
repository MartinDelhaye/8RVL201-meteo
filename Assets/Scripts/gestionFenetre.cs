using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

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
    public RenderTexture textureCamEte;
    public RenderTexture textureCamNeige;

    [Header("Cameras Paysage")]
    public Camera cameraEte;
    public Camera cameraNeige;
    private Skybox skyboxEte;
    private Skybox skyboxNeige;

    [Header("Objet à modifier")]
    public Renderer planeRenderer;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip rainSound;
    public AudioClip snowSound;
    public AudioClip sunSound;
    public AudioClip cloudSound;

    [Header("Lighting")]
    public Light lightPaysage;

    void Start()
    {
        WeatherManager.OnWeatherReady += Init;
        skyboxEte = cameraEte.GetComponent<Skybox>();
        skyboxNeige = cameraNeige.GetComponent<Skybox>();
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

    void PlaySound(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    void UpdateMeteo()
    {
        int codeMeteo = WeatherController.Instance.weatherCode;
        int hour = WeatherController.Instance.currentHourIndex;

        bool isDay = (hour >= 6 && hour < 18);

        bool isSun = (codeMeteo >= 0 && codeMeteo <= 19);
        bool isCloud = (codeMeteo >= 20 && codeMeteo <= 49);
        bool isSnow = (codeMeteo >= 70 && codeMeteo <= 79);
        bool isRain = (codeMeteo >= 50 && codeMeteo <= 69) || (codeMeteo >= 80 && codeMeteo <= 99);

        // 🔹 Particules
        rainParticles.SetActive(true);
        snowParticles.SetActive(true);

        if (isRain)
        {
            rainPS.Play();
            snowPS.Stop();
            PlaySound(rainSound);
        }
        else if (isSnow)
        {
            rainPS.Stop();
            snowPS.Play();
            PlaySound(snowSound);
        }
        else if (isCloud)
        {
            rainPS.Stop();
            snowPS.Stop();
            PlaySound(cloudSound);
        }
        else if (isSun)
        {
            rainPS.Stop();
            snowPS.Stop();
            PlaySound(sunSound);
        }

        // 🔹 SKYBOX + TEXTURE (avec gestion nuit)
        if (!isDay)
        {
            skyboxEte.material = skyNight;
            skyboxNeige.material = skyNight;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
        else if (isSnow)
        {
            skyboxEte.material = skyRain;
            skyboxNeige.material = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", textureCamNeige);
        }
        else if (isRain)
        {
            skyboxEte.material = skyRain;
            skyboxNeige.material = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
        else if (isCloud)
        {
            skyboxEte.material = skyCloud;
            skyboxNeige.material = skyCloud;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
        else if (isSun)
        {
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
            skyboxEte.material = skySun;
            skyboxNeige.material = skySun;
        }

        if (lightPaysage != null)
        {
            lightPaysage.enabled = true;

            float t = hour / 23f;
            float angle = Mathf.Lerp(0f, 180f, t);

            lightPaysage.transform.rotation = Quaternion.Euler(angle, -90f, -90f);

            lightPaysage.intensity = 5;
        }

        // 🔥 Update lumière globale
        DynamicGI.UpdateEnvironment();
    }
}