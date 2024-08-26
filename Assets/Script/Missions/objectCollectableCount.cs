using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class objectCollectableCount : MonoBehaviour
{
    public int count = 0;
    [SerializeField] ObjectifMission objectifMission;

    public void IncrementeMission1()
    {
        count++;
        objectifMission.ShowTextMotivation();
        if (count <= 11)
        {
            objectifMission.UpdateObjectif(0, 1);
        }
        else if (count >= 11 && count <= 17)
        {
            objectifMission.UpdateObjectif(1, 1);
        }
        else if (count >= 17 && count <= 20)
        {
            objectifMission.UpdateObjectif(2, 1);
        }
    }

    public void IncrementeMission2()
    {
        objectifMission.UpdateObjectif(0, 1);
        objectifMission.ShowTextMotivation();
    }
}
