using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckVagueIA : MonoBehaviour
{
    [SerializeField] List<GameObject> iaVagues = new List<GameObject>();
    [SerializeField] ObjectifMission objectifMission;
    [SerializeField] private GameObject Kinematic;
    private string actualVague;
    private bool renderIA;
    private GameObject player;
    private int initialCount;
    public int indexVague;
    private bool antiSpamKinematicSpawn = false;

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
                        player = FindInactiveObjectByName("MainCharacter(Clone)");
                        child.gameObject.SetActive(true);
                        child.gameObject.GetComponent<AIPlayerController>().AssignPlayerTransforms(player);
                    }
                    count++;
                }
            }
        }
        renderIA = false;
        return count;
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
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
                    if (SceneManager.GetActiveScene().name == "Mission5")
                    {
                        if (actualVague == "IAVague1" && !antiSpamKinematicSpawn)
                        {
                            player = GameObject.FindGameObjectWithTag("Player");
                            if (player != null)
                                player.SetActive(false);
                            Instantiate(Kinematic);
                            antiSpamKinematicSpawn = true;
                            break;
                        }
                    }
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
