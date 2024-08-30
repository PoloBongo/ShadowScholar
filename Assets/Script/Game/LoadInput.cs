using Invector;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using Invector.vItemManager;
using Invector.vShooter;
using System.IO;
using UnityEngine;

public class LoadInput : MonoBehaviour
{
    private vThirdPersonInput invectorInput;
    private vShooterMeleeInput invectorShooterInput;
    private vMeleeCombatInput invectorMeleeCombatInput;
    private vInventory vInventory;
    private vSwimming vSwimming;
    private vCoverController vCoverController;
    private vZipLine vZipLine;
    private vLadderAction vLadderAction;
    private vDrawHideShooterWeapons vDrawHideShooterWeapons;
    private MissionManager missionManager;
    private ZoneLoader zoneLoader;

    private string filePath;
    [SerializeField] private JsonFile jsonFile;
    private void Start()
    {
        ReadJson();
    }

    private void ReadJson()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            jsonFile.ReadJsonFile(filePath);
        }
    }

    void OnEnable()
    {
        ReadJson();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            OnThirdPersonInputActivated();
        }
    }

    public void OnThirdPersonInputActivated()
    {
        invectorInput = FindObjectOfType<vThirdPersonInput>();
        if (invectorInput == null) Debug.LogError("vThirdPersonInput non trouvé.");
        invectorShooterInput = FindAnyObjectByType<vShooterMeleeInput>();
        if (invectorShooterInput == null) Debug.LogError("vShooterMeleeInput non trouvé.");
        invectorMeleeCombatInput = FindAnyObjectByType<vMeleeCombatInput>();
        if (invectorMeleeCombatInput == null) Debug.LogError("invectorMeleeCombatInput non trouvé.");
        vInventory = FindAnyObjectByType<vInventory>();
        if (vInventory == null) Debug.LogError("vInventory non trouvé.");
        vSwimming = FindAnyObjectByType<vSwimming>();
        if (vSwimming == null) Debug.LogError("vSwimming non trouvé.");
        vCoverController = FindAnyObjectByType<vCoverController>();
        if (vCoverController == null) Debug.LogError("vCoverController non trouvé.");
        vZipLine = FindAnyObjectByType<vZipLine>();
        if (vZipLine == null) Debug.LogError("vZipLine non trouvé.");
        vLadderAction = FindAnyObjectByType<vLadderAction>();
        if (vLadderAction == null) Debug.LogError("vLadderAction non trouvé.");
        vDrawHideShooterWeapons = FindAnyObjectByType<vDrawHideShooterWeapons>();
        if (vDrawHideShooterWeapons == null) Debug.LogError("vDrawHideShooterWeapons non trouvé.");
        zoneLoader = FindAnyObjectByType<ZoneLoader>();

        GameObject gameController = GameObject.Find("GameController");
        if (gameController != null)
            missionManager = gameController.GetComponent<MissionManager>();

        if (invectorInput != null || invectorMeleeCombatInput != null || invectorShooterInput != null || vInventory != null || vSwimming != null || vCoverController != null || vZipLine != null)
        {
            invectorInput.jumpInput = new GenericInput(jsonFile.shadowScholar.inputSettings.jumpInput, "X", "X");
            invectorInput.rollInput = new GenericInput(jsonFile.shadowScholar.inputSettings.rollInput, "B", "B");
            invectorInput.sprintInput = new GenericInput(jsonFile.shadowScholar.inputSettings.sprintInput, "LeftStickClick", "LeftStickClick");
            invectorInput.crouchInput = new GenericInput(jsonFile.shadowScholar.inputSettings.crouchInput, "Y", "Y");

            invectorMeleeCombatInput.weakAttackInput = new GenericInput(jsonFile.shadowScholar.inputSettings.weakAttackInput, "RB", "RB");
            invectorMeleeCombatInput.strongAttackInput = new GenericInput(jsonFile.shadowScholar.inputSettings.strongAttackInput, "RT", "RT");
            invectorMeleeCombatInput.blockInput = new GenericInput(jsonFile.shadowScholar.inputSettings.blockInput, "LB", "LB");

            invectorShooterInput.aimInput = new GenericInput(jsonFile.shadowScholar.inputSettings.aimInput, "LT", "LT");
            invectorShooterInput.shotInput = new GenericInput(jsonFile.shadowScholar.inputSettings.shootInput, "RT", "RT");
            invectorShooterInput.reloadInput = new GenericInput(jsonFile.shadowScholar.inputSettings.reloadInput, "LB", "LB");
            invectorShooterInput.scopeViewInput = new GenericInput(jsonFile.shadowScholar.inputSettings.scopeViewInput, "RB", "RB");

            invectorInput.strafeInput = new GenericInput(jsonFile.shadowScholar.inputSettings.strafeInput, "RightStickClick", "RightStickClick");
            invectorShooterInput.switchCameraSideInput = new GenericInput(jsonFile.shadowScholar.inputSettings.switchCameraSideInput, "RightStickClick", "RightStickClick");
       
            vInventory.openInventory = new GenericInput(jsonFile.shadowScholar.inputSettings.openInventoryInput, "Back", "Back");
            vInventory.cancel = new GenericInput(jsonFile.shadowScholar.inputSettings.closeInventoryInput, "B", "B");

            vSwimming.swimUpInput = new GenericInput(jsonFile.shadowScholar.inputSettings.swimUpInput, "X", "X");
            vSwimming.swimDownInput = new GenericInput(jsonFile.shadowScholar.inputSettings.swimDownInput, "Y", "Y");

            vCoverController.enterExitInput = new GenericInput(jsonFile.shadowScholar.inputSettings.coverInput, "A", "A");

            vZipLine.exitZipline = new GenericInput(jsonFile.shadowScholar.inputSettings.exitZipLineInput, "X", "X");

            vLadderAction.enterInput = new GenericInput(jsonFile.shadowScholar.inputSettings.enterInput, "A", "A");
            vLadderAction.exitInput = new GenericInput(jsonFile.shadowScholar.inputSettings.exitInput, "B", "B");
            vLadderAction.fastClimbInput = new GenericInput(jsonFile.shadowScholar.inputSettings.fastClimbInput, "LeftStickClick", "LeftStickClick");
            vLadderAction.slideDownInput = new GenericInput(jsonFile.shadowScholar.inputSettings.slideDownClimbInput, "X", "X");

            vDrawHideShooterWeapons.hideAndDrawWeaponsInput = new GenericInput(jsonFile.shadowScholar.inputSettings.hideWeaponInput, "LB", "LB");

            if (missionManager != null)
            {
                missionManager.openObjectifInput = new GenericInput(jsonFile.shadowScholar.inputSettings.openObjectifInput, "Start", "Start");
                missionManager.closeObjectifInput = new GenericInput(jsonFile.shadowScholar.inputSettings.closeObjectifInput, "Start", "Start");
            }

            if (zoneLoader != null)
            {
                zoneLoader.openCloseMenuTPInput = new GenericInput(jsonFile.shadowScholar.inputSettings.openCloseMenuTPInput, "Start", "Start");
            }
        }
        else
        {
            Debug.LogError("Component Invector non trouvé même après activation.");
        }
    }
}
