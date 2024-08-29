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
    [System.Serializable]
    public class TextMotivation
    {
        public GameObject textGameobject;
        public GameObject parentText;
    }
    [SerializeField] private List<TextMotivation> listTexts = new List<TextMotivation>();
    [SerializeField] private GameObject objectifRequiredText;
    [SerializeField] private GameObject objectifRequiredTextParent;
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

    public void UpdateFinalObjectif(int indexObjectif, int maxValue)
    {
        if (indexObjectif >= 0 && indexObjectif < listObjectifs.Count)
        {
            listObjectifs[indexObjectif].objectifValue = maxValue;
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

    public bool CheckPrecisObjectif(int indexObjectif)
    {
        if (listObjectifs[indexObjectif].actualValue == listObjectifs[indexObjectif].objectifValue)
        {
            return true;
        }
        return false;
    }

    public void ShowTextMotivation()
    {
        StartCoroutine(WaitForTextWhenObjectifIsInProgress());
    }

    IEnumerator WaitForTextWhenObjectifIsInProgress()
    {
        int randomText = UnityEngine.Random.Range(0, listTexts.Count);
        for (int i = 0; i < listTexts.Count;i++)
        {
            if (i == randomText)
            {
                listTexts[randomText].textGameobject.SetActive(true);
                listTexts[randomText].parentText.SetActive(true);
            }
        }
        yield return new WaitForSeconds(3f);

        listTexts[randomText].textGameobject.SetActive(false);
        listTexts[randomText].parentText.SetActive(false);
    }

    public void ShowTextNeedObjectif()
    {
        StartCoroutine(WaitForTextWhenObjectifIsRequired());
    }

    IEnumerator WaitForTextWhenObjectifIsRequired()
    {
        objectifRequiredTextParent.SetActive(true);
        objectifRequiredText.SetActive(true);
        yield return new WaitForSeconds(3f);

        objectifRequiredTextParent.SetActive(false);
        objectifRequiredText.SetActive(false);
    }
}
