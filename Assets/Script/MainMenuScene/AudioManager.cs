using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Slider volumeSlider;
    private float currentVolume = 1f;
    private string saveFilePath;

    private void Awake()
    {
        // Singleton Pattern pour que l'AudioManager soit accessible de toutes les scènes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "audioSettings.json");
    }

    private void Start()
    {
        LoadVolumeSettings();
        ApplyVolume();
    }

    public void OnVolumeSliderChanged()
    {
        currentVolume = volumeSlider.value;
        ApplyVolume();
        SaveVolumeSettings();
    }

    private void ApplyVolume()
    {
        AudioListener.volume = currentVolume;
    }

    private void SaveVolumeSettings()
    {
        VolumeSettings settings = new VolumeSettings { volume = currentVolume };
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadVolumeSettings()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(json);
            currentVolume = settings.volume;
            if (volumeSlider != null)
            {
                volumeSlider.value = currentVolume;
            }
        }
    }
}

[System.Serializable]
public class VolumeSettings
{
    public float volume;
}
