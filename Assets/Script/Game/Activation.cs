using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Activation : MonoBehaviour
{
    private GameObject loadInput;
    private GameObject zoneLoader;
    private MinimapIcon icon;
    private List<GameObject> interactionScripts = new List<GameObject>();
    private List<MinimapIcon> minimapIcons = new List<MinimapIcon>();
    public string sceneName;
    
    // Start is called before the first frame update
    void Start()
    {
        ActivateScript();
    }

    public void ActivateScript()
    {
        loadInput = GameObject.Find("LoadInput");
        loadInput.GetComponent<LoadInput>().OnThirdPersonInputActivated();
        minimapIcons.Add(GetComponent<MinimapIcon>());


        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            MinimapIcon game = gameObject.GetComponent<MinimapIcon>();
            if (game != null)
            {
                minimapIcons.Add(gameObject.GetComponent<MinimapIcon>());
            }
            

        }

        foreach (MinimapIcon icon in minimapIcons)
        {
            icon.InitMinimapIcon();
        }

        if (sceneName == "Game")
        {
            zoneLoader = GameObject.Find("ZoneLoader");
            loadInput.GetComponent<ZoneLoader>().InitZoneLoader();

            GameObject acceuil = GameObject.Find("ReceptionTable_straight (3)");
            GameObject launchMission = GameObject.Find("Chair_PD_01_02_Interact");
            if (acceuil != null)
            {
                interactionScripts.Add(acceuil);
            }

            if (launchMission != null)
            {
                interactionScripts.Add(launchMission);
            }

            foreach (GameObject obj in interactionScripts)
            {
                InteractionScript interactionScript = obj.GetComponent<InteractionScript>();
                if (interactionScript != null)
                {
                    interactionScript.InitInteractionScript();
                }

                GetCardDialogue getCardDialogue = obj.GetComponent<GetCardDialogue>();
                if (getCardDialogue != null)
                {
                    getCardDialogue.InitGetCardDialogue();
                }

                MissionHub missionHub = obj.GetComponent<MissionHub>();
                if (missionHub != null)
                {
                    missionHub.InitMissionHub();
                }
            }
        }
        else if (sceneName == "Mission2")
        {
            GameObject barril = GameObject.Find("vBarrel_C");
            GameObject barril2 = GameObject.Find("vBarrel_C (1)");
            GameObject barril3 = GameObject.Find("vBarrel_C (2)");
            GameObject barril4 = GameObject.Find("vBarrel_C (3)");
            GameObject cameraPlayer = GameObject.FindGameObjectWithTag("MainCamera");

            if (barril != null && barril2 != null && barril3 != null && barril4 != null)
            {
                interactionScripts.Add(barril);
                interactionScripts.Add(barril2);
                interactionScripts.Add(barril3);
                interactionScripts.Add(barril4);
            }

            foreach (GameObject obj in interactionScripts)
            {
                DetectionObject detectionObject = obj.GetComponent<DetectionObject>();
                if (detectionObject != null)
                {
                    detectionObject.InitDetectionObject(cameraPlayer.GetComponent<Camera>(), true);
                }
            }
        }
        else if (sceneName == "Mission3")
        {
            GameObject IAVague1 = GameObject.Find("IAVague1");
            foreach (Transform child in IAVague1.transform)
            {
                AIPlayerController aiController = child.GetComponent<AIPlayerController>();
                if (aiController != null)
                {
                    aiController.AssignPlayerTransforms(this.gameObject);
                }
            }
        }
    }
}
