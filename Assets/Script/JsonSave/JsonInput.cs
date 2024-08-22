using System.IO;
using TMPro;
using UnityEngine;

public class JsonInput : MonoBehaviour
{
    [SerializeField] JsonFile jsonFile;
    #region TMP InputField
    [System.Serializable]
    public class TMP_InputFieldClass
    {
        public TMP_InputField jumpInputText;
        public TMP_InputField rollInputText;
        public TMP_InputField sprintInputText;
        public TMP_InputField crouchInputText;

        public TMP_InputField weakAttackInputText;
        public TMP_InputField strongAttackInputText;
        public TMP_InputField blockInputText;

        public TMP_InputField aimInputText;
        public TMP_InputField shootInputText;
        public TMP_InputField reloadInputText;
        public TMP_InputField scopeViewInputText;

        public TMP_InputField strafeInputText;
        public TMP_InputField switchCameraSideInputText;

        public TMP_InputField openInventoryInputText;
        public TMP_InputField closeInventoryInputText;

        public TMP_InputField swiumUpInputText;
        public TMP_InputField swiumDownInputText;

        public TMP_InputField coverInputText;

        public TMP_InputField exitZipLineInputText;

        public TMP_InputField enterLadderInputText;
        public TMP_InputField exitInputText;
        public TMP_InputField fastClimbInputText;
        public TMP_InputField slideDownInputText;

        public TMP_InputField hideWeaponInputText;
    }
    public TMP_InputFieldClass tmp_InputFieldClass;
    #endregion
    private string filePath;
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            jsonFile.ReadJsonFile(filePath);
            setAllInputText();
        }
        else
        {
            jsonFile.CreateJsonFile();
            setAllInputText();
        }
    }

    private void setAllInputText()
    {
        tmp_InputFieldClass.jumpInputText.text = jsonFile.shadowScholar.inputSettings.jumpInput;
        tmp_InputFieldClass.rollInputText.text = jsonFile.shadowScholar.inputSettings.rollInput;
        tmp_InputFieldClass.sprintInputText.text = jsonFile.shadowScholar.inputSettings.sprintInput;
        tmp_InputFieldClass.crouchInputText.text = jsonFile.shadowScholar.inputSettings.crouchInput;

        tmp_InputFieldClass.weakAttackInputText.text = jsonFile.shadowScholar.inputSettings.weakAttackInput;
        tmp_InputFieldClass.strongAttackInputText.text = jsonFile.shadowScholar.inputSettings.strongAttackInput;
        tmp_InputFieldClass.blockInputText.text = jsonFile.shadowScholar.inputSettings.blockInput;

        tmp_InputFieldClass.aimInputText.text = jsonFile.shadowScholar.inputSettings.aimInput;
        tmp_InputFieldClass.shootInputText.text = jsonFile.shadowScholar.inputSettings.shootInput;
        tmp_InputFieldClass.reloadInputText.text = jsonFile.shadowScholar.inputSettings.reloadInput;
        tmp_InputFieldClass.scopeViewInputText.text = jsonFile.shadowScholar.inputSettings.scopeViewInput;

        tmp_InputFieldClass.strafeInputText.text = jsonFile.shadowScholar.inputSettings.strafeInput;
        tmp_InputFieldClass.switchCameraSideInputText.text = jsonFile.shadowScholar.inputSettings.switchCameraSideInput;

        tmp_InputFieldClass.openInventoryInputText.text = jsonFile.shadowScholar.inputSettings.openInventoryInput;
        tmp_InputFieldClass.closeInventoryInputText.text = jsonFile.shadowScholar.inputSettings.closeInventoryInput;

        tmp_InputFieldClass.swiumUpInputText.text = jsonFile.shadowScholar.inputSettings.swimUpInput;
        tmp_InputFieldClass.swiumDownInputText.text = jsonFile.shadowScholar.inputSettings.swimDownInput;

        tmp_InputFieldClass.coverInputText.text = jsonFile.shadowScholar.inputSettings.coverInput;

        tmp_InputFieldClass.exitZipLineInputText.text = jsonFile.shadowScholar.inputSettings.exitZipLineInput;

        tmp_InputFieldClass.enterLadderInputText.text = jsonFile.shadowScholar.inputSettings.enterInput;
        tmp_InputFieldClass.exitInputText.text = jsonFile.shadowScholar.inputSettings.exitInput;
        tmp_InputFieldClass.fastClimbInputText.text = jsonFile.shadowScholar.inputSettings.fastClimbInput;
        tmp_InputFieldClass.slideDownInputText.text = jsonFile.shadowScholar.inputSettings.slideDownClimbInput;

        tmp_InputFieldClass.hideWeaponInputText.text = jsonFile.shadowScholar.inputSettings.hideWeaponInput;
    }
}