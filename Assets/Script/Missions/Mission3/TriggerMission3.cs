using UnityEngine;

public class TriggerMission3 : MonoBehaviour
{
    [SerializeField] ObjectifMission objectifMission;
    [SerializeField] GameObject previousStepMission3;
    [SerializeField] GameObject nextStepMission3;
    private bool alreadyEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectifMission.CheckPrecisObjectif(0))
            {
                if (!alreadyEnter)
                {
                    objectifMission.UpdateObjectif(2, 1);

                    nextStepMission3.SetActive(true);
                    previousStepMission3.SetActive(true);

                    GameObject IABoss = GameObject.Find("IABoss");
                    foreach (Transform child in IABoss.transform)
                    {
                        objectifMission.UpdateFinalObjectif(0, 16);
                        child.gameObject.SetActive(true);
                    }

                    alreadyEnter = true;
                }
            }
            else
            {
                objectifMission.ShowTextNeedObjectif();
            }
        }
    }
}
