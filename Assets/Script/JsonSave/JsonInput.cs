using System.Collections.Generic;
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

        public TMP_InputField openObjectifInputText;
        public TMP_InputField closeObjectifInputText;

        public TMP_InputField openCloseMenuTPInputText;
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
        tmp_InputFieldClass.jumpInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.jumpInput);
        tmp_InputFieldClass.rollInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.rollInput);
        tmp_InputFieldClass.sprintInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.sprintInput);
        tmp_InputFieldClass.crouchInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.crouchInput);

        tmp_InputFieldClass.weakAttackInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.weakAttackInput);
        tmp_InputFieldClass.strongAttackInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.strongAttackInput);
        tmp_InputFieldClass.blockInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.blockInput);

        tmp_InputFieldClass.aimInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.aimInput);
        tmp_InputFieldClass.shootInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.shootInput);
        tmp_InputFieldClass.reloadInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.reloadInput);
        tmp_InputFieldClass.scopeViewInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.scopeViewInput);

        tmp_InputFieldClass.strafeInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.strafeInput);
        tmp_InputFieldClass.switchCameraSideInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.switchCameraSideInput);

        tmp_InputFieldClass.openInventoryInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.openInventoryInput);
        tmp_InputFieldClass.closeInventoryInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.closeInventoryInput);

        tmp_InputFieldClass.swiumUpInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.swimUpInput);
        tmp_InputFieldClass.swiumDownInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.swimDownInput);

        tmp_InputFieldClass.coverInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.coverInput);

        tmp_InputFieldClass.exitZipLineInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.exitZipLineInput);

        tmp_InputFieldClass.enterLadderInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.enterInput);
        tmp_InputFieldClass.exitInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.exitInput);
        tmp_InputFieldClass.fastClimbInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.fastClimbInput);
        tmp_InputFieldClass.slideDownInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.slideDownClimbInput);

        tmp_InputFieldClass.hideWeaponInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.hideWeaponInput);

        tmp_InputFieldClass.openObjectifInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.openObjectifInput);
        tmp_InputFieldClass.closeObjectifInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.closeObjectifInput);

        tmp_InputFieldClass.openCloseMenuTPInputText.text = ConvertAzertyToQwerty(jsonFile.shadowScholar.inputSettings.openCloseMenuTPInput);
    }

    private string ConvertAzertyToQwerty(string inputKey)
    {
        Dictionary<string, string> azertyToQwerty = new Dictionary<string, string>()
        {
            {"A", "Q" }, {"Q", "A"},
            {"1", "KEYPAD1" }, {"KEYPAD1", "1"},
            {"2", "KEYPAD2" }, {"KEYPAD2", "2"},
            {"3", "KEYPAD3" }, {"KEYPAD3", "3"},
            {"4", "KEYPAD4" }, {"KEYPAD4", "4"},
            {"5", "KEYPAD5" }, {"KEYPAD5", "5"},
            {"6", "KEYPAD6" }, {"KEYPAD6", "6"},
            {"7", "KEYPAD7" }, {"KEYPAD7", "7"},
            {"8", "KEYPAD8" }, {"KEYPAD8", "8"},
            {"9", "KEYPAD9" }, {"KEYPAD9", "9"},
            {"0", "KEYPAD0" }, {"KEYPAD0", "0"},
            {"&", "ALPHA1" }, {"ALPHA1", "&"},
            {"é", "ALPHA2" }, {"ALPHA2", "é"},
            {"(", "ALPHA5" }, {"ALPHA5", "("},
            {"-", "ALPHA6" }, {"ALPHA6", "-"},
            {"è", "ALPHA7" }, {"ALPHA7", "è"},
            {"_", "ALPHA8" }, {"ALPHA8", "_"},
            {"ç", "ALPHA9" }, {"ALPHA9", "ç"},
            {"à", "ALPHA0" }, {"ALPHA0", "à"},
            {")", "MINUS" }, {"MINUS", ")"},
            {"=", "EQUALS" }, {"EQUALS", "="},
            {"!", "/" }, {"/", "!"},
            {"Z", "W" }, {"W", "Z"},
            {"M", ";" }, {";", "M"},
            {"ù", "'" }, {"'", "ù"},
            {":", "." }, {".", ":"},
        };

        if (azertyToQwerty.ContainsKey(inputKey))
        {
            return azertyToQwerty[inputKey];
        }
        return inputKey;
    }

}