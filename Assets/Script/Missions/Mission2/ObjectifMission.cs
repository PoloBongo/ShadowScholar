using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifMission : MonoBehaviour
{
    [System.Serializable]
    public class Objectif
    {
        public string objectifName;
        public int actualValue;
        public int objectifValue;

        public Objectif(string objectifName, int actualValue, int objectifValue)
        {
            objectifName = objectifName;
            actualValue = actualValue;
            objectifValue = objectifValue;
        }
    }
    [SerializeField] int indexMission;
    [SerializeField] private List<Objectif> listObjectifs = new List<Objectif>();
    [SerializeField] private MissionManager missionManager;

    public void UpdateObjectif(int indexObjectif, int value)
    {
        if (indexObjectif >= 0 && indexObjectif < listObjectifs.Count)
        {
            listObjectifs[indexObjectif].actualValue += value;
            string valueString = listObjectifs[indexObjectif].actualValue.ToString();
            missionManager.SetTextObjectifMission(indexObjectif, valueString);
            CheckObjectif();
        }
        else
        {
            Debug.LogWarning("index pas trouvé");
        }
    }

    private void CheckObjectif()
    {
        int totalObjectifCompleted = 0;
        for (int i = 0; i < listObjectifs.Count; i++)
        {
            if (listObjectifs[i].actualValue == listObjectifs[i].objectifValue)
            {
                totalObjectifCompleted++;
            }
            Debug.Log("total objectif completed : " + totalObjectifCompleted);
            if (totalObjectifCompleted == listObjectifs.Count)
            {
                missionManager.MissionStatus("Success", indexMission);
            }
        }
    }
}
