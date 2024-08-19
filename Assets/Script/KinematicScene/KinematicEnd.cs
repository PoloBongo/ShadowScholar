using UnityEngine;
using UnityEngine.Playables;  // N�cessaire pour utiliser le PlayableDirector

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
        // Obtenir le PlayableDirector attach� � ce GameObject
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            // Abonner une fonction � l'�v�nement "stopped"
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector n'a pas �t� trouv� sur le GameObject.");
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
        Debug.Log("La cin�matique est termin�e !");
    }

    private void OnDestroy()
    {
        // Se d�sabonner de l'�v�nement pour �viter les fuites de m�moire
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
