using System.Collections.Generic;
using UnityEngine;

public class CheckVagueIA : MonoBehaviour
{
    [SerializeField] List<GameObject> iaVagues = new List<GameObject>();
    [SerializeField] ObjectifMission objectifMission;
    [SerializeField] private bool canSuccessMission;
    private string actualVague;
    private bool renderIA;
    private GameObject player;
    private int initialCount;
    private int indexVague;

    public void InitCheckIA()
    {
        actualVague = iaVagues[0].name;
        initialCount = iaVagues[0].transform.childCount;
        indexVague = 0;
        renderIA = false;
    }

    private int SearchIAInVague()
    {
        int count = 0;
        for (int i = 0; i < iaVagues.Count; i++)
        {
            if (iaVagues[i].name == actualVague)
            {
                foreach(Transform child in iaVagues[i].transform)
                {
                    if (renderIA)
                    {
                        player = GameObject.FindGameObjectWithTag("Player");
                        child.gameObject.SetActive(true);
                        child.gameObject.GetComponent<AIPlayerController>().AssignPlayerTransforms(player);
                    }
                    count++;
                }
            }
        }
        Debug.Log(count);
        renderIA = false;
        return count;
    }

    private void CheckAndSwitchVague()
    {
        int count = SearchIAInVague();
        if (count < initialCount && count >= 0)
        {
            initialCount--;
            objectifMission.UpdateObjectif(indexVague, 1);
        }

        if (count <= 0)
        {
            for (int i = 0; i < iaVagues.Count; i++)
            {
                if (actualVague == iaVagues[i].name)
                {
                    if (actualVague == "IABoss")
                        objectifMission.UpdateObjectif(1, 1);
                    if (i + 1 < iaVagues.Count)
                    {
                        actualVague = iaVagues[i + 1].name;
                        renderIA = true;
                        initialCount = iaVagues[i + 1].transform.childCount;
                        if (iaVagues[i].name != "IAVague1")
                            objectifMission.ShowTextMotivation();
                    }
                    break;
                }
            }
        }
    }

    void Update()
    {
        CheckAndSwitchVague();
    }
}
