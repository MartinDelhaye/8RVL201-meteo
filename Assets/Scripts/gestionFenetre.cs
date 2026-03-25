using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GestionFenetre : MonoBehaviour
{
    private int lastCode = -1; // code météo par défaut
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

        int code = WeatherController.Instance.weatherCode;
        int hour = WeatherController.Instance.currentHourIndex;

        UpdateWeatherVisuals(code);
        UpdateTimeVisuals(hour);
    }

void Update()
{
    if (WeatherController.Instance == null) return;

    int currentCode = WeatherController.Instance.weatherCode;
    int currentHour = WeatherController.Instance.currentHourIndex;

    if (currentCode != lastCode)
    {
        lastCode = currentCode;
        UpdateWeatherVisuals(currentCode);
    }
    UpdateTimeVisuals(currentHour);
}

void UpdateWeatherVisuals(int codeMeteo)
{
    bool isSun = (codeMeteo >= 0 && codeMeteo <= 19);
    bool isCloud = (codeMeteo >= 20 && codeMeteo <= 49);
    bool isSnow = (codeMeteo >= 70 && codeMeteo <= 79);
    bool isRain = (codeMeteo >= 50 && codeMeteo <= 69) || (codeMeteo >= 80 && codeMeteo <= 99);

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

    // skybox météo (jour seulement)
    if (isSnow)
    {
        skyboxNeige.material = skyRain;
        planeRenderer.material.SetTexture("_BaseMap", textureCamNeige);
    }
    else if (isRain)
    {
        skyboxEte.material = skyRain;
        planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
    }
    else if (isCloud)
    {
        skyboxEte.material = skyCloud;
        planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
    }
    else if (isSun)
    {
        skyboxEte.material = skySun;
        planeRenderer.material.SetTexture("_BaseMap", textureCamEte);
    }
}

void UpdateTimeVisuals(int hour)
{
    bool isDay = (hour >= 6 && hour < 18);

    if (!isDay)
    {
        skyboxEte.material = skyNight;
        skyboxNeige.material = skyNight;
    }

    if (lightPaysage != null)
    {
        float angle = (hour / 23f) * 180f;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateLight(angle));
    }
}
IEnumerator RotateLight(float targetAngle)
{
    float duration = 3f;
    Quaternion startRotation = lightPaysage.transform.rotation;
    Quaternion targetRotation = Quaternion.Euler(
        targetAngle,
        lightPaysage.transform.eulerAngles.y,
        lightPaysage.transform.eulerAngles.z
    );
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


void PlaySound(AudioClip clip)
{
    if (audioSource.clip == clip && audioSource.isPlaying) return;

    audioSource.Stop();
    audioSource.clip = clip;
    audioSource.Play();
}
}