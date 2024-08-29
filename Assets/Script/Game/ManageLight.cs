using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageLight : MonoBehaviour
{
    private Light directionalLight;
    private Light[] lampadaireLights;
    private float night = 0.4f;
    private float day = 0.6f;
    private bool statusLamps = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Mission1")
        {
            directionalLight = GetComponent<Light>();
            lampadaireLights = FindObjectsOfType<Light>();
            lampadaireLights = System.Array.FindAll(lampadaireLights, lamp => lamp.gameObject != directionalLight.gameObject);
        }
    }

    private void Update()
    {
        if (directionalLight.intensity < night && !statusLamps)
        {
            SetLampadairesState(true);
            statusLamps = true;
        } 
        else if (directionalLight.intensity > day && statusLamps)
        {
            SetLampadairesState(false);
            statusLamps = false;
        }
    }

    private void SetLampadairesState(bool state)
    {
        foreach (Light lampadaireLight in lampadaireLights)
        {
            if (lampadaireLight != null)
            {
                lampadaireLight.enabled = state;
            }
        }
    }
}