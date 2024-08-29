using GeNa.Core;
using System.IO;
using UnityEngine;

public class Horloge : MonoBehaviour
{
    private float duration = 3600f;
    private float durationLight = 300f;
    private float startTime;
    private float lightStartTime;
    private float lightStartTimeDay;
    private bool timeAdjustedManually = false;

    [SerializeField] Light directionalLight;
    private GameObject foundDirectionalLight;
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;
    [SerializeField] private GaiaTimeOfDayLightSync interiorSpot;
    [SerializeField] private GaiaTimeOfDayLightSync interiorSpotVariant;
    public float percentageComplete;

    private GameObject jsonFileGamObject;
    private JsonFile jsonFile;
    private string filePath;

    void Start()
    {
        startTime = Time.time;
        jsonFileGamObject = GameObject.Find("Save");
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");
        if (jsonFileGamObject != null )
        {
            jsonFile = jsonFileGamObject.GetComponent<JsonFile>();
            if (jsonFile != null)
            {
                jsonFile.ReadJsonFile(filePath);
                SetTimeManually(jsonFile.shadowScholar.horloge.time);
            }
        }

        if (directionalLight == null)
        {
            foundDirectionalLight = GameObject.Find("Directional Light");
            directionalLight = foundDirectionalLight.GetComponent<Light>();
        }
    }

    void Update()
    {
        if (!timeAdjustedManually)
        {
            float elapsedTime = Time.time - startTime;
            percentageComplete = elapsedTime / duration;
        }
        else
        {
            startTime = Time.time - (percentageComplete * duration);
            timeAdjustedManually = false;
        }
        float angle = Mathf.Lerp(0f, -720f, percentageComplete);
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, angle);

        if (percentageComplete > 0.37f)
        {
            SetIntensityNight(percentageComplete, 0.37f, 1f, 0f);
        }
        
        if (percentageComplete > 0.826f)
        {
            SetIntensityDay(percentageComplete, 0.826f, 0f, 1f);
        }

        if (percentageComplete >= 1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            startTime = Time.time;
            lightStartTime = 0f;
            lightStartTimeDay = 0f;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("ta appuyé sur K");
            interiorSpot.m_overrideSystemActiveState = true;
            interiorSpot.m_lightComponent.enabled = true;

            interiorSpotVariant.m_overrideSystemActiveState = true;
            interiorSpotVariant.m_lightComponent.enabled = true;
        }
    }

    public void SetTimeManually(float newPercentage)
    {
        percentageComplete = newPercentage;
        timeAdjustedManually = true;
    }

    private void SetIntensityNight(float percentageComplete, float time, float start, float end)
    {
        if (percentageComplete > time)
        {
            if (lightStartTime == 0f)
            {
                lightStartTime = Time.time;
                ChangeSkybox(nightSkybox);
            }
            float elapsedLightTime = Time.time - lightStartTime;
            float percentageIntensity = elapsedLightTime / durationLight;
            float intensity = Mathf.Lerp(start, end, percentageIntensity);
            directionalLight.intensity = intensity;

            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(start, 0.5f, percentageIntensity));
            DynamicGI.UpdateEnvironment();
        }
    }

    private void SetIntensityDay(float percentageComplete, float time, float start, float end)
    {
        if (percentageComplete > time)
        {
            if (lightStartTimeDay == 0f)
            {
                lightStartTimeDay = Time.time;
                ChangeSkybox(daySkybox);
            }
            float elapsedLightTime = Time.time - lightStartTimeDay;
            float percentageIntensity = elapsedLightTime / durationLight;
            float intensity = Mathf.Lerp(start, end, percentageIntensity);
            directionalLight.intensity = intensity;

            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(0.5f, end, percentageIntensity));
            DynamicGI.UpdateEnvironment();
        }
    }

    private void ChangeSkybox(Material newSkybox)
    {
        if (newSkybox != null)
        {
            RenderSettings.skybox = newSkybox;
            DynamicGI.UpdateEnvironment();
        }
    }

    void OnApplicationQuit()
    {
        jsonFile.shadowScholar.horloge.time = percentageComplete;
        jsonFile.SaveJson();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
