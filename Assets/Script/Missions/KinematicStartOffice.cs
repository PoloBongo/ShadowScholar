using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class KinematicStartOffice : MonoBehaviour
{
    private PlayableDirector playableDirector;
    [SerializeField] private GameObject kinematic;
    private Pause pause;
    private GameObject player;
    private GameObject kinematicPackage;
    private JsonFile jsonFile;
    private string filePath;

    private bool isCinematicCompleted = false;
    private bool isApplicationQuitting = false;
    public void StartKinematicOffice()
    {
        GameObject pauseGamobject = GameObject.Find("Pause");
        if (pauseGamobject != null)
            pause = pauseGamobject.GetComponent<Pause>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.SetActive(false);
        }
        GameObject jsonFileObject = GameObject.Find("Save");
        jsonFile = jsonFileObject.GetComponent<JsonFile>();
        if (jsonFile != null)
        {
            filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

            if (File.Exists(filePath))
            {
                jsonFile.ReadJsonFile(filePath);
            }
        }
        kinematicPackage = Instantiate(kinematic);

        foreach (Transform child in kinematicPackage.transform)
        {
            GameObject childObject = child.gameObject;
            PlayableDirector director = childObject.GetComponent<PlayableDirector>();

            if (director != null)
            {
                playableDirector = director;
                break;
            }
        }

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector n'a pas été trouvé sur le GameObject.");
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (isApplicationQuitting || isCinematicCompleted)
        {
            return;
        }

        player.SetActive(true);
        jsonFile.shadowScholar.kinematicOffice.isFinish = true;
        jsonFile.SaveJson();
        pause.enabled = true;
        isCinematicCompleted = true;
        Destroy(kinematicPackage);
        Debug.Log("La cinématique est terminée !");
    }

    private void OnDestroy()
    {
        // Se désabonner de l'événement pour éviter les fuites de mémoire
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
}
