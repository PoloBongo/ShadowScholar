using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KinematicEndMission1 : MonoBehaviour
{
    private PlayableDirector playableDirector;
    [SerializeField] GameObject transitionGame;
    [SerializeField] GameObject destroyKinematic;
    [SerializeField] GameObject doorInTheKinematic;

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
        doorInTheKinematic.SetActive(true);
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
