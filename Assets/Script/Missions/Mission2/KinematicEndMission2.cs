using UnityEngine;
using UnityEngine.Playables;

public class KinematicEndMission2 : MonoBehaviour
{
    private PlayableDirector playableDirector;
    [SerializeField] GameObject transitionGame;
    [SerializeField] GameObject destroyKinematic;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector n'a pas �t� trouv� sur le GameObject.");
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        Destroy(destroyKinematic);
        transitionGame.SetActive(true);
        Debug.Log("La cin�matique est termin�e !");
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
