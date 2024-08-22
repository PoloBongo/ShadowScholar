using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class objectCollectableCount : MonoBehaviour
{
    public int count = 0;
    [SerializeField] JsonFile jsonFile;
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");
        if (jsonFile != null)
        {
            jsonFile.ReadJsonFile(filePath);
        }
    }

    public void Incremente()
    {
        count++;
        if (count >= 20)
        {
            jsonFile.shadowScholar.missions.mission1.isFinish = true;
            jsonFile.SaveJson();
            SceneManager.LoadScene(2);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
    }
}
