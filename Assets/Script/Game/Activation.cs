using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    private GameObject loadInput;
    private GameObject zoneLoader;
    private GameObject[] interactionScripts = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        ActivateScript();
    }

    public void ActivateScript()
    {
        loadInput = GameObject.Find("LoadInput");
        loadInput.GetComponent<LoadInput>().OnThirdPersonInputActivated();

        zoneLoader = GameObject.Find("ZoneLoader");
        loadInput.GetComponent<ZoneLoader>().InitZoneLoader();

        interactionScripts[0] = GameObject.Find("ReceptionTable_straight (3)");
        if (interactionScripts[0] != null)
        {
            InteractionScript interactionScript1 = interactionScripts[0].GetComponent<InteractionScript>();
            GetCardDialogue getCardDialogue1 = interactionScripts[0].GetComponent<GetCardDialogue>();

            interactionScript1.InitInteractionScript();
            getCardDialogue1.InitGetCardDialogue();
        }

        interactionScripts[1] = GameObject.Find("Chair_PD_01_02_Interact");
        if (interactionScripts[1] != null)
        {
            InteractionScript interactionScript1 = interactionScripts[1].GetComponent<InteractionScript>();
            MissionHub missionHub1 = interactionScripts[1].GetComponent<MissionHub>();

            interactionScript1.InitInteractionScript();
            missionHub1.InitMissionHub();
        }
    }
}
