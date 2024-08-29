using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEventForVMessageReceiver : MonoBehaviour
{
    bool lose = false;

    public vMessageReceiver vMessageReceiver;
    public MissionManager missionManager;
    [SerializeField] int indexMission;

    public void IsLose()
    {
        missionManager.MissionStatus("Failed", indexMission);
    }
}
