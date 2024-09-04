using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vItemManager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionHub : MonoBehaviour
{
    public InteractionScript interactionScript;

    private GameObject player;
    private Animator animator;
    private vThirdPersonController vThirdPersonController;
    private vShooterMeleeInput vShooterMeleeInput;
    private Rigidbody rigidbody;
    private GameObject mainCamera;
    private vThirdPersonCamera vThirdPersonCamera;

    private bool isInteracting = false;
    private bool hasMove = false;
    private bool hasRotate = false;
    private bool isInDesk = false;
    private bool animIsFinish = false;
    public Transform playerTargetTransform;
    public Transform chairOriginTransform;
    public Transform chairTargetTransform;

    public Transform cameraOriginTransform;
    public Transform cameraTargetTransform1;
    public Transform cameraTargetTransform2;
    private bool cameraInPosition1 = false;
    private bool cameraInPosition2 = false;

    public GameObject screen;
    private Material screenMaterial;
    private Color originalEmissionColor;
    private float emissionFactor = 0f;

    [Serializable]
    public class HubInterface
    {
        public TextMeshPro missionHubTitle;
        public GameObject missionCloseButton;
        public TextMeshPro missionTitle;
        public TextMeshPro missionText;
        public GameObject missionLocalization;
        public TextMeshPro missionObjectifTitle;
        public TextMeshPro missionObjectifText;
        public GameObject missionLaunchButton;
        public TextMeshPro missionLaunchText;
    }

    public HubInterface hubInterface = new HubInterface();
    private bool interfaceIsShow = false;
    private Material missionCloseButtonMaterial;
    private Material missionLocalizationMaterial;
    private Material missionLaunchButtonMaterial;
    private int layerMask;

    private bool isLeaving = false;


    private List<IMission> missions = new List<IMission>();
    private int nextMissionNum;
    private int nextMissionInfo = 1;
    Mission1 mission1;
    Mission2 mission2;
    Mission3 mission3;
    Mission4 mission4;
    Mission5 mission5;

    private GameObject jsonSaveGameObject;
    private JsonFile jsonSave;

    void Start()
    {
        screenMaterial = screen.GetComponent<MeshRenderer>().material;
        screenMaterial.SetColor("_EmissionColor", Color.black);
        originalEmissionColor = Color.white;

        missionCloseButtonMaterial = hubInterface.missionCloseButton.GetComponent<Renderer>().material;
        missionLocalizationMaterial = hubInterface.missionLocalization.GetComponent<Renderer>().material;
        missionLaunchButtonMaterial = hubInterface.missionLaunchButton.GetComponent<Renderer>().material;

        layerMask = LayerMask.GetMask("UI");
        ChangeNextMissionNum(1);
    }

    public void InitMissionHub()
    {
        if (interactionScript != null)
        {
            interactionScript.OnInteract += OpenHub;
        }
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponent<Animator>();
        vThirdPersonController = player.GetComponent<vThirdPersonController>();
        vShooterMeleeInput = player.GetComponent<vShooterMeleeInput>();
        rigidbody = player.GetComponent<Rigidbody>();

        mainCamera = GameObject.Find("vThirdPersonCamera");
        vThirdPersonCamera = mainCamera.GetComponent<vThirdPersonCamera>();

        jsonSaveGameObject = GameObject.Find("Save");
        jsonSave = jsonSaveGameObject.GetComponent<JsonFile>();

        SetInfoActualMission();
    }

    private void SetInfoActualMission()
    {
        if (!jsonSave.shadowScholar.missions.mission1.isFinish)
        {
            mission1 = new Mission1(1, "Initiation eu combat", "Finissez les 3 �tapes de l'entra�nement", "Mission1/Localization");
            missions.Add(mission1);
            nextMissionInfo = 1;
        }
        else
        {
            if (!jsonSave.shadowScholar.missions.mission2.isFinish)
            {
                mission2 = new Mission2(2, "Rep�rage dans les favelas", "Rep�rer les points et les objets strat�giques sur place", "Mission2/Localization");
                missions.Add(mission2);
                nextMissionInfo = 2;
            }
            else
            {
                if (!jsonSave.shadowScholar.missions.mission3.isFinish)
                {
                    mission3 = new Mission3(3, "Attaquez la planque du gang des KUMI 415", "Trouvez les caisses d'armes", "Mission3/Localization");
                    missions.Add(mission3);
                    nextMissionInfo = 3;
                }
                else
                {
                    if (!jsonSave.shadowScholar.missions.mission4.isFinish)
                    {
                        mission4 = new Mission4(4, "Rep�rage du cartel", "Rep�rer les routines du cartel", "Mission4/Localization");
                        missions.Add(mission4);
                        nextMissionInfo = 4;
                    }
                    else
                    {
                        if (!jsonSave.shadowScholar.missions.mission5.isFinish)
                        {
                            mission5 = new Mission5(5, "Attaquez la villa du cartel", "Neutralisez le cartel et r�cup�rer les caisses d'armes", "Mission5/Localization");
                            missions.Add(mission5);
                            nextMissionInfo = 5;
                        }
                    }
                }
            }
        }

        ChangeNextMissionNum(1);
    }

    void OnDestroy()
    {
        if (interactionScript != null)
        {
            interactionScript.OnInteract -= OpenHub;
        }
    }

    void OpenHub()
    {
        vShooterMeleeInput.SetLockAllInput(true);
        vShooterMeleeInput.SetLockCameraInput(true);
        vThirdPersonController.StopCharacter();
        vThirdPersonCamera.FreezeCamera();
        isInteracting = true;
        interactionScript.enabled = false;
        cameraOriginTransform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
    }

    void Update()
    {
        if (!isInteracting)
        {
            return;
        }
        if (isLeaving)
        {
            LeavingHUB();
        }
        else
        {
            OpeningHUB();
        }

    }

    void LeavingHUB()
    {
        if (interfaceIsShow)
        {
            if (emissionFactor > 0f)
            {
                HideHubInterface();
                return;
            }
            else
            {
                interfaceIsShow = false;
            }
        }

        if (cameraInPosition2)
        {
            return;
        }

        if (isInDesk)
        {
            float angle = Quaternion.Angle(transform.rotation, chairOriginTransform.rotation);
            float distance = Vector3.Distance(transform.position, chairOriginTransform.position);
            if (angle > 1f)
            {
                RotateTowardsTarget(transform, chairOriginTransform.rotation, 1);
                player.transform.rotation = playerTargetTransform.rotation;
                player.transform.position = playerTargetTransform.position;
            }
            if (distance > 0.1f)
            {
                MoveTowardsTarget(transform, chairOriginTransform.position, 1);
                MoveTowardsTarget(player.transform, chairOriginTransform.position, 1);
            }

            if (angle <= 1f && distance <= 0.1f)
            {
                isInDesk = false;
                player.GetComponent<CapsuleCollider>().enabled = true;
                rigidbody.useGravity = true;
                animator.Play("Standing-Sit");
                StartCoroutine(WaitForStandingAnimationToEnd());
            }
            else
            {
                return;
            }
        }

        if(animIsFinish) 
        {
            return;
        }

        ResetState();
        animator.Play("Pick_Mid", 0, 0f);

    }

    void OpeningHUB()
    {
        if (!hasMove)
        {
            float distance = Vector3.Distance(player.transform.position, playerTargetTransform.position);
            if (distance > 0.1f)
            {
                vThirdPersonController.MoveToPosition(playerTargetTransform.position);
                return;
            }
            else
            {
                vThirdPersonController.StopCharacter();
                hasMove = true;
            }
        }

        if (!hasRotate)
        {
            float angle = Quaternion.Angle(player.transform.rotation, playerTargetTransform.rotation);
            if (angle > 1f)
            {
                RotateTowardsTarget(player.transform, playerTargetTransform.rotation, 3);
                return;
            }
            else
            {
                hasRotate = true;
                animator.Play("Sit");
                StartCoroutine(WaitForSitAnimationToEnd());
            }
        }


        if (!isInDesk && animIsFinish)
        {
            float angle = Quaternion.Angle(transform.rotation, chairTargetTransform.rotation);
            float distance = Vector3.Distance(transform.position, chairTargetTransform.position);
            if (angle > 1f)
            {
                RotateTowardsTarget(transform, chairTargetTransform.rotation, 1);
                player.transform.rotation = playerTargetTransform.rotation;
                player.transform.position = playerTargetTransform.position;
            }
            if (distance > 0.1f)
            {
                MoveTowardsTarget(transform, chairTargetTransform.position, 1);
                MoveTowardsTarget(player.transform, chairTargetTransform.position, 1);
            }

            if (angle <= 1f && distance <= 0.1f)
            {
                isInDesk = true;
                player.GetComponent<CapsuleCollider>().enabled = false;
                rigidbody.useGravity = false;
            }
            else
            {
                return;
            }
        }
        else if (!animIsFinish)
        {
            return;
        }

        player.transform.position = playerTargetTransform.position;

        if (!cameraInPosition1)
        {
            return;
        }
        if (!interfaceIsShow)
        {
            if (emissionFactor < 1f)
            {
                ShowHubInterface();
                return;
            }
            else
            {
                interfaceIsShow = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        if (!cameraInPosition2)
        {
            return;
        }
        CheckUserAction();
    }


    void CheckUserAction()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject == hubInterface.missionCloseButton ) 
                {
                    isLeaving = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = false;
                    StartCoroutine(WaitForCameraPosition(mainCamera.transform, cameraTargetTransform1));
                }
                else if(clickedObject == hubInterface.missionLaunchButton )
                {
                    hubInterface.missionLaunchText.fontSize = 0.17f;
                    hubInterface.missionLaunchText.text = "Lancement en cours...";
                    SaveInventoryWhenMissionIsLaunch();
                    switch (nextMissionInfo)
                    {
                        case 1:
                            if (!jsonSave.shadowScholar.missions.mission1.isFinish)
                                jsonSave.shadowScholar.missions.isStart = true;
                            break;
                        case 2:
                            if (!jsonSave.shadowScholar.missions.mission2.isFinish)
                                jsonSave.shadowScholar.missions.isStart = true;
                            break;
                        case 3:
                            if (!jsonSave.shadowScholar.missions.mission3.isFinish)
                                jsonSave.shadowScholar.missions.isStart = true;
                            break;
                        case 4:
                            if (!jsonSave.shadowScholar.missions.mission4.isFinish)
                                jsonSave.shadowScholar.missions.isStart = true;
                            break;
                        case 5:
                            if (!jsonSave.shadowScholar.missions.mission5.isFinish)
                                jsonSave.shadowScholar.missions.isStart = true;
                            break;
                    }
                    jsonSave.shadowScholar.player.position = new Vector3(1605.767f, 15.61f, 646.4249f);
                    jsonSave.SaveJson();
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    SceneManager.LoadScene("SceneChargement");
                }
            }
        }
    }

    private GameObject FindActiveObjectByName(string name)
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

    private void SaveInventoryWhenMissionIsLaunch()
    {
        GameObject player = FindActiveObjectByName("MainCharacter(Clone)");
        if (player != null)
        {
            vItemManager vItemManagerObject = player.GetComponent<vItemManager>();
            if (vItemManagerObject != null)
            {
                vItemManagerObject.SaveInventory();
                vItemManagerObject.LoadInventory();
            }
        }
    }

    void RotateTowardsTarget(Transform transform, Quaternion targetRotation, float rotationSpeed)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveTowardsTarget(Transform transform, Vector3 targetPosition, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void HideHubInterface()
    {
        emissionFactor -= 0.05f;
        screenMaterial.SetColor("_EmissionColor", originalEmissionColor * emissionFactor);
        DynamicGI.SetEmissive(screen.GetComponent<MeshRenderer>(), originalEmissionColor * emissionFactor);

        hubInterface.missionHubTitle.color = new Color(hubInterface.missionHubTitle.color.r, hubInterface.missionHubTitle.color.g, hubInterface.missionHubTitle.color.b, emissionFactor);
        missionCloseButtonMaterial.color = new Color(missionCloseButtonMaterial.color.r, missionCloseButtonMaterial.color.g, missionCloseButtonMaterial.color.b, emissionFactor);
        hubInterface.missionTitle.color = new Color(hubInterface.missionTitle.color.r, hubInterface.missionTitle.color.g, hubInterface.missionTitle.color.b, emissionFactor);
        hubInterface.missionText.color = new Color(hubInterface.missionText.color.r, hubInterface.missionText.color.g, hubInterface.missionText.color.b, emissionFactor);
        missionLocalizationMaterial.color = new Color(missionLocalizationMaterial.color.r, missionLocalizationMaterial.color.g, missionLocalizationMaterial.color.b, emissionFactor);
        hubInterface.missionObjectifTitle.color = new Color(hubInterface.missionObjectifTitle.color.r, hubInterface.missionObjectifTitle.color.g, hubInterface.missionObjectifTitle.color.b, emissionFactor);
        hubInterface.missionObjectifText.color = new Color(hubInterface.missionObjectifText.color.r, hubInterface.missionObjectifText.color.g, hubInterface.missionObjectifText.color.b, emissionFactor);
        missionLaunchButtonMaterial.color = new Color(missionLaunchButtonMaterial.color.r, missionLaunchButtonMaterial.color.g, missionLaunchButtonMaterial.color.b, emissionFactor);
        hubInterface.missionLaunchText.color = new Color(hubInterface.missionLaunchText.color.r, hubInterface.missionLaunchText.color.g, hubInterface.missionLaunchText.color.b, emissionFactor);
            

    }

    void ShowHubInterface()
    {
        emissionFactor += 0.05f;
        screenMaterial.SetColor("_EmissionColor", originalEmissionColor * emissionFactor);
        DynamicGI.SetEmissive(screen.GetComponent<MeshRenderer>(), originalEmissionColor * emissionFactor);

        hubInterface.missionHubTitle.color = new Color(hubInterface.missionHubTitle.color.r, hubInterface.missionHubTitle.color.g, hubInterface.missionHubTitle.color.b, emissionFactor);
        missionCloseButtonMaterial.color = new Color(missionCloseButtonMaterial.color.r, missionCloseButtonMaterial.color.g, missionCloseButtonMaterial.color.b, emissionFactor);
        hubInterface.missionTitle.color = new Color(hubInterface.missionTitle.color.r, hubInterface.missionTitle.color.g, hubInterface.missionTitle.color.b, emissionFactor);
        hubInterface.missionText.color = new Color(hubInterface.missionText.color.r, hubInterface.missionText.color.g, hubInterface.missionText.color.b, emissionFactor);
        missionLocalizationMaterial.color = new Color(missionLocalizationMaterial.color.r, missionLocalizationMaterial.color.g, missionLocalizationMaterial.color.b, emissionFactor);
        hubInterface.missionObjectifTitle.color = new Color(hubInterface.missionObjectifTitle.color.r, hubInterface.missionObjectifTitle.color.g, hubInterface.missionObjectifTitle.color.b, emissionFactor);
        hubInterface.missionObjectifText.color = new Color(hubInterface.missionObjectifText.color.r, hubInterface.missionObjectifText.color.g, hubInterface.missionObjectifText.color.b, emissionFactor);
        missionLaunchButtonMaterial.color = new Color(missionLaunchButtonMaterial.color.r, missionLaunchButtonMaterial.color.g, missionLaunchButtonMaterial.color.b, emissionFactor);
        hubInterface.missionLaunchText.color = new Color(hubInterface.missionLaunchText.color.r, hubInterface.missionLaunchText.color.g, hubInterface.missionLaunchText.color.b, emissionFactor);
    }

    void ResetState()
    {
        isInteracting = false;
        hasMove = false;
        hasRotate = false;
        isInDesk = false;
        animIsFinish = false;
        cameraInPosition1 = false;
        cameraInPosition2 = false;
        interfaceIsShow = false;
        isLeaving = false;
        vShooterMeleeInput.SetLockAllInput(false);
        vShooterMeleeInput.SetLockCameraInput(false);
        vThirdPersonCamera.UnFreezeCamera();
        interactionScript.enabled = true;
    }

    void ChangeNextMissionNum(int missionNum)
    {
        if (missionNum < 1 || missionNum > missions.Count)
        {
            // out of range chef
        }
        else
        {
            nextMissionNum = missionNum;
            if (hubInterface != null && hubInterface.missionText != null)
            {
                hubInterface.missionText.text = missions[missionNum - 1].name;
            }
            if (missions[missionNum - 1] != null && missions[missionNum - 1].localization_texture != null)
            {
                if (missionLocalizationMaterial != null)
                    missionLocalizationMaterial.SetTexture("_MainTex", missions[missionNum - 1].localization_texture);
            }
            hubInterface.missionObjectifText.text = missions[missionNum - 1].objectif;
        }
    }

    IEnumerator WaitForSitAnimationToEnd()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Sit"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        animIsFinish = true;
        StartCoroutine(WaitForCameraPosition(mainCamera.transform, cameraTargetTransform1));
    }

    IEnumerator WaitForStandingAnimationToEnd()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Standing-Sit"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        animIsFinish = false;
    }

    IEnumerator WaitForCameraPosition(Transform transform, Transform targetTransform)
    {
        while(transform.position !=  targetTransform.position)
        {
            MoveTowardsTarget(transform, targetTransform.position, 1);
            Vector3 directionToTarget = screen.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            RotateTowardsTarget(transform, targetRotation, 2);
            yield return null;
        }
        if (targetTransform == cameraTargetTransform1 && !isLeaving) 
        {
            cameraInPosition1 = true;
            StartCoroutine(WaitForCameraPosition(mainCamera.transform, cameraTargetTransform2));

        }
        else if (targetTransform == cameraTargetTransform2 && !isLeaving)
        {
            cameraInPosition2 = true;
        }
        else if(targetTransform == cameraTargetTransform1 && isLeaving)
        {
            cameraInPosition2 = false;
            StartCoroutine(WaitForCameraPosition(mainCamera.transform, cameraOriginTransform));
        }
        else if(targetTransform == cameraOriginTransform && isLeaving)
        {
            cameraInPosition1 = false;
        }
    }
}
