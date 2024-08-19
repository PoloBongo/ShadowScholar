using System.IO;
using UnityEngine;

public class JsonFile : MonoBehaviour
{
    [System.Serializable]
    public class ShadowScholar
    {
        public KinematicStart kinematicStart;
    }
    [System.Serializable]
    public class KinematicStart
    {
        public bool isFinish;
    }

    private string filePath;
    private ShadowScholar shadowScholar;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            ReadJsonFile();
        }
        else
        {
            CreateJsonFile();
        }
    }

    void CreateJsonFile()
    {
        shadowScholar = new ShadowScholar()
        {
            kinematicStart = new KinematicStart()
            {
                isFinish = false
            }
        };

        SaveJson();
        Debug.Log("Fichier json crée");
    }

    void ReadJsonFile()
    {
        string json = File.ReadAllText(filePath);
        shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);
        Debug.Log("Boolean : " + shadowScholar.kinematicStart.isFinish);
    }

    void SaveJson()
    {
        string json = JsonUtility.ToJson(shadowScholar, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Données save");
    }

    public bool ReadKinematicStartJsonFile()
    {
        string json = File.ReadAllText(filePath);
        shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);
        Debug.Log("Boolean : " + shadowScholar.kinematicStart.isFinish);

        return shadowScholar.kinematicStart.isFinish;
    }

    public void UpdateKinematicStartDataJson(bool _isFinish)
    {
        if (shadowScholar != null)
        {
            shadowScholar.kinematicStart.isFinish = _isFinish;

            SaveJson();

            Debug.Log("Données update");
        } else { Debug.Log("shadowScholar est null"); }
    }
}
