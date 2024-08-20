using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ButtonStartKinematic : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private PlayableDirector playableDirector;

    private GameObject mainCharacter;
    [SerializeField] bool isForKinematicScene;
    // Start is called before the first frame update
    void Start()
    {
        Button playKinematic = GetComponent<Button>();
        playKinematic.onClick.AddListener(TaskOnClick);
        if (isForKinematicScene )
        {
            mainCharacter = GameObject.Find("MainCharacterKinematic");

            if (mainCharacter != null)
            {
                playableDirector = mainCharacter.GetComponent<PlayableDirector>();
            }
        }
    }

    private void TaskOnClick()
    {
        ObjectToDisabled();
        if (isForKinematicScene)
        {
            PlayKinematic();
        }
        
    }

    private void ObjectToDisabled()
    {
        objectToDisable.SetActive(false);
    }

    private void PlayKinematic()
    {
        playableDirector.Play();
    }
}
