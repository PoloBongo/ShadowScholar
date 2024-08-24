using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class objectCollectableCount : MonoBehaviour
{
    public int count = 0;
    private string filePath;
    [SerializeField] JsonFile jsonFile;
    [SerializeField] GameObject missionSuccess;
    private GameObject hudPlayer;

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
            jsonFile.shadowScholar.missions.isStart = false;
            jsonFile.SaveJson();
            if (missionSuccess != null)
                hudPlayer = GameObject.Find("InvectorComponents");
                StartCoroutine(MissionSuccess());
        }
    }

    private IEnumerator MissionSuccess()
    {
        if (hudPlayer != null)
            hudPlayer.SetActive(false);
        missionSuccess.SetActive(true);
        yield return new WaitForSeconds(3);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }
}
