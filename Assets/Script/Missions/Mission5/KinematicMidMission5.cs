using UnityEngine;
using UnityEngine.Playables;
using static JsonFile;

public class KinematicEndMission5 : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private GameObject nextVague;
    private GameObject previousVague;
    [SerializeField] GameObject destroyKinematic;
    private GameObject player;

    void Start()
    {
        nextVague = FindInactiveObjectByName("IAVague_Step2");
        previousVague = GameObject.Find("IAVague_Step1");
        player = FindInactiveObjectByName("MainCharacter(Clone)");

        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector n'a pas été trouvé sur le GameObject.");
        }
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        Destroy(previousVague);
        nextVague.GetComponent<CheckVagueIA>().InitCheckIA();
        nextVague.SetActive(true);
        nextVague.GetComponent<CheckVagueIA>().enabled = true;
        nextVague.GetComponent<CheckVagueIA>().indexVague = 2;

        GameObject IABoss = GameObject.Find("IABoss");
        foreach (Transform child in IABoss.transform)
        {
            child.gameObject.SetActive(true);
        }
        if (player != null)
        {
            player.transform.position = new Vector3(1820.908f, 14.99999f, 160.3557f);
            player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            player.SetActive(true);
        }
        Destroy(destroyKinematic);
        Debug.Log("La cinématique est terminée !");
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
