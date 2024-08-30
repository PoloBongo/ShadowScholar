using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField] private GameObject missionFailedUI;
    [SerializeField] private GameObject missionSuccessUI;
    [SerializeField] private GameObject objectifUI;
    [SerializeField] List<TMP_Text> objectifs = new List<TMP_Text>();
    [SerializeField] private int indexMission;
    private GameObject HUDPlayer;
    private vThirdPersonController vThirdPersonController;
    public GenericInput openObjectifInput = new GenericInput("F1", "Start", "Start");
    public GenericInput closeObjectifInput = new GenericInput("Escape", "Start", "Start");
    private bool isOpen = false;

    private string filePath;
    [SerializeField] JsonFile jsonFile;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");
        if (jsonFile != null)
        {
            jsonFile.ReadJsonFile(filePath);
        }
    }

    public void InitPlayerMissionManager()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        vThirdPersonController = player.GetComponent<vThirdPersonController>();
    }

    public void MissionStatus(string missionStatus, int indexMission)
    {
        HUDPlayer = GameObject.Find("InvectorComponents");
        if (missionStatus == "Success")
        {
            SaveMissionFinish(indexMission);
        }
        jsonFile.shadowScholar.missions.isStart = false;
        jsonFile.SaveJson();
        StartCoroutine(WaitEndScreenMission(missionStatus));
    }

    IEnumerator WaitEndScreenMission(string missionStatus)
    {
        if (HUDPlayer != null)
        {
            HUDPlayer.SetActive(false);
        }

        yield return null;

        if (missionStatus != null || missionStatus != "")
        {
            if (missionStatus == "Failed")
                missionFailedUI.SetActive(true);
            else
                missionSuccessUI.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }

    private void OpenObjectifFunction(bool isOpen)
    {
        objectifUI.SetActive(isOpen);
    }

    private void Update()
    {
        if (openObjectifInput.GetButtonDown())
        {
            isOpen = !isOpen;
            OpenObjectifFunction(isOpen);
        }
        if (closeObjectifInput.GetButtonDown())
        {
            isOpen = false;
            OpenObjectifFunction(isOpen);
        }

        if (vThirdPersonController != null)
        {
            if (vThirdPersonController.currentHealth <= 0)
            {
                MissionStatus("Failed", indexMission);
            }
        }
    }

    public void SetTextObjectifMission(int idObjectif, string textObjectif)
    {
        for (int i = 0; i < objectifs.Count;i++)
        {
            if (objectifs[idObjectif])
            {
                objectifs[idObjectif].text = textObjectif;
            }
        }
    }

    private void SaveMissionFinish(int indexMission)
    {
        switch (indexMission)
        {
            case 0:
                jsonFile.shadowScholar.missions.mission1.isFinish = true;
                break;
            case 1:
                jsonFile.shadowScholar.missions.mission2.isFinish = true;
                break;
            case 2:
                jsonFile.shadowScholar.missions.mission3.isFinish = true;
                break;
        }
        jsonFile.SaveJson();
    }
}
