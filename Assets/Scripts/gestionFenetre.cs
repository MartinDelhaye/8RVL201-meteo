using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GestionFenetre : MonoBehaviour
{
    private int lastCode = -1;
    private int lastHour = -1;
    private Material lastSkyboxDay;
    private Coroutine rotateCoroutine;

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
        audioSource.volume = 2f;
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

        int currentHour = WeatherController.Instance.currentHourIndex;
        if (currentHour != lastHour)
        {
            lastHour = currentHour;
            UpdateHour();
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
        if (isSnow)
        {
            skyboxNeige.material = skyRain;
            lastSkyboxDay = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", textureCamNeige);
        }
        else if (isRain)
        {
            skyboxEte.material = skyRain;
            lastSkyboxDay = skyRain;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
        else if (isCloud)
        {
            skyboxEte.material = skyCloud;
            lastSkyboxDay = skyCloud;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
        else if (isSun)
        {
            skyboxEte.material = skySun;
            lastSkyboxDay = skySun;
            planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
        }
    }

    void UpdateHour()
    {
        int hour = WeatherController.Instance.currentHourIndex;
        bool isDay = (hour >= 6 && hour < 18);

        if (!isDay)
        {
            skyboxEte.material = skyNight;
        }
        else
        {
            if (lastSkyboxDay != null)
                skyboxEte.material = lastSkyboxDay;
            else
                skyboxEte.material = skySun;
        }

        if (lightPaysage != null)
        {
            lightPaysage.enabled = true;
            float angle = (hour / 23f) * 180f;
            if (rotateCoroutine != null)
                StopCoroutine(rotateCoroutine);
            rotateCoroutine = StartCoroutine(RotateLight(angle));
            lightPaysage.intensity = 5;
        }
        DynamicGI.UpdateEnvironment();
    }

    IEnumerator RotateLight(float targetAngle)
    {
        float duration = 3f;
        Quaternion startRotation = lightPaysage.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetAngle, -90f, -90f);

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            lightPaysage.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            time += Time.deltaTime;
            yield return null;
        }
        lightPaysage.transform.rotation = targetRotation;
    }
}

