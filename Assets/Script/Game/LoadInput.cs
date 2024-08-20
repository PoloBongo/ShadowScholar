using Invector;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using Invector.vItemManager;
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
    private vDrawHideMeleeWeapons vDrawHideMeleeWeapons;

    private string filePath;
    [SerializeField] private JsonFile jsonFile;
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            jsonFile.ReadJsonFile();
            Debug.Log(jsonFile.shadowScholar.inputSettings.jumpInput);
        }
    }

    public void OnThirdPersonInputActivated()
    {
        invectorInput = FindObjectOfType<vThirdPersonInput>();
        invectorShooterInput = FindAnyObjectByType<vShooterMeleeInput>();
        invectorMeleeCombatInput = FindAnyObjectByType<vShooterMeleeInput>();
        vInventory = FindAnyObjectByType<vInventory>();
        vSwimming = FindAnyObjectByType<vSwimming>();
        vCoverController = FindAnyObjectByType<vCoverController>();
        vZipLine = FindAnyObjectByType<vZipLine>();

        if (invectorInput != null || invectorMeleeCombatInput != null || invectorShooterInput != null)
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
            invectorShooterInput.scopeViewInput = new GenericInput(jsonFile.shadowScholar.inputSettings.scopViewInput, "RB", "RB");

            invectorInput.strafeInput = new GenericInput(jsonFile.shadowScholar.inputSettings.strafeInput, "RightStickClick", "RightStickClick");
            invectorShooterInput.switchCameraSideInput = new GenericInput(jsonFile.shadowScholar.inputSettings.switchCameraSideInput, "RightStickClick", "RightStickClick");
       
            vInventory.openInventory = new GenericInput(jsonFile.shadowScholar.inputSettings.openInventoryInput, "Back", "Back");
            vInventory.cancel = new GenericInput(jsonFile.shadowScholar.inputSettings.closeInventoryInput, "B", "B");

            vSwimming.swimUpInput = new GenericInput(jsonFile.shadowScholar.inputSettings.swimUpInput, "X", "X");
            vSwimming.swimDownInput = new GenericInput(jsonFile.shadowScholar.inputSettings.swimDownInput, "Y", "Y");

            vCoverController.enterExitInput = new GenericInput(jsonFile.shadowScholar.inputSettings.coverInput, "A", "A");

            vZipLine.exitZipline = new GenericInput(jsonFile.shadowScholar.inputSettings.exitZipLineInput, "X", "X");

            vLadderAction.enterInput = new GenericInput(jsonFile.shadowScholar.inputSettings.enterLadderInput, "A", "A");
            vLadderAction.exitInput = new GenericInput(jsonFile.shadowScholar.inputSettings.exitLadderInput, "B", "B");
            vLadderAction.fastClimbInput = new GenericInput(jsonFile.shadowScholar.inputSettings.fastClimbInput, "LeftStickClick", "LeftStickClick");
            vLadderAction.slideDownInput = new GenericInput(jsonFile.shadowScholar.inputSettings.slideDownClimbInput, "X", "X");

            vDrawHideMeleeWeapons.hideAndDrawWeaponsInput = new GenericInput(jsonFile.shadowScholar.inputSettings.hideWeaponInput, "LB", "LB");
        }
        else
        {
            Debug.LogError("Component Invector non trouvé même après activation.");
        }
    }
}
