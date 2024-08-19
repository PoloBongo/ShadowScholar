using UnityEngine;
using UnityEngine.Playables;  // Nécessaire pour utiliser le PlayableDirector

public class KinematicEnd : MonoBehaviour
{
    private PlayableDirector playableDirector;
    [SerializeField] JsonFile jsonFile;
    [SerializeField] LoadManager loadManager;
    [SerializeField] GameObject transitionGame;

    private bool isCinematicCompleted = false;
    private bool isApplicationQuitting = false;

    void Start()
    {
        // Obtenir le PlayableDirector attaché à ce GameObject
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            // Abonner une fonction à l'événement "stopped"
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

        isCinematicCompleted = true;

        jsonFile.UpdateKinematicStartDataJson(true);
        transitionGame.SetActive(true);
        loadManager.StartLoadManager();
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
