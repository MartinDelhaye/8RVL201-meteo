using UnityEngine;
using System.Collections;

public class GestionFenetre  : MonoBehaviour
{
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
    {
        // ⏳ attendre que l'API réponde
        while (!RecupMeteo.Instance.isDataRecuperee)
        {
            yield return null;
        }

        UpdateMeteo();
    }

    void UpdateMeteo()
    {
        int codeMeteo = 30;
        bool isDay = RecupMeteo.Instance.isDay;

        bool isSun = (codeMeteo >= 0 && codeMeteo <= 19);
        bool isCloud = (codeMeteo >= 20 && codeMeteo <= 49);
        bool isSnow = (codeMeteo >= 70 && codeMeteo <= 79);
        bool isRain = (codeMeteo >= 50 && codeMeteo <= 69) || (codeMeteo >= 80 && codeMeteo <= 99);

        if (isSnow)
        {
            RenderSettings.skybox = skyRain;

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

        // 🔥 important pour la lumière
        DynamicGI.UpdateEnvironment();
    }
}