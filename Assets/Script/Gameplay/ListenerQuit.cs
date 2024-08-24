using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerQuit : MonoBehaviour
{
    [SerializeField] JsonFile jsonFile;
    [SerializeField] bool isMainMenu;

    private void Start()
    {
        if (isMainMenu)
            jsonFile.shadowScholar.missions.isStart = false;
            jsonFile.SaveJson();
    }
    void OnApplicationQuit()
    {
        if (jsonFile != null)
            jsonFile.shadowScholar.missions.isStart = false;
            jsonFile.SaveJson();
    }
}
