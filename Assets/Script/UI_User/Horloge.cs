using UnityEngine;

public class Horloge : MonoBehaviour
{
    private float duration = 60f;
    private float durationLight = 10f;
    private float startTime;
    private float lightStartTime;
    private float lightStartTimeDay;
    [SerializeField] Light directionalLight;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;
        float percentageComplete = elapsedTime / duration;
        float angle = Mathf.Lerp(0f, -720f, percentageComplete);
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, angle);


        if (percentageComplete > 0.2f)
        {
            SetIntensityNight(percentageComplete, 0.2f, 1f, 0.1f);
        }
        
        if (percentageComplete > 0.6f)
        {
            SetIntensityDay(percentageComplete, 0.6f, 0.1f, 1f);
        }

        if (percentageComplete >= 1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            startTime = Time.time;
            lightStartTime = 0f;
            lightStartTimeDay = 0f;
        }
    }

    private void SetIntensityNight(float percentageComplete, float time, float start, float end)
    {
        if (percentageComplete > time)
        {
            if (lightStartTime == 0f)
            {
                lightStartTime = Time.time;
            }
            float elapsedLightTime = Time.time - lightStartTime;
            float percentageIntensity = elapsedLightTime / durationLight;
            float intensity = Mathf.Lerp(start, end, percentageIntensity);
            directionalLight.intensity = intensity;
        }
    }

    private void SetIntensityDay(float percentageComplete, float time, float start, float end)
    {
        if (percentageComplete > time)
        {
            if (lightStartTimeDay == 0f)
            {
                lightStartTimeDay = Time.time;
            }
            float elapsedLightTime = Time.time - lightStartTimeDay;
            float percentageIntensity = elapsedLightTime / durationLight;
            float intensity = Mathf.Lerp(start, end, percentageIntensity);
            directionalLight.intensity = intensity;
        }
    }
}
