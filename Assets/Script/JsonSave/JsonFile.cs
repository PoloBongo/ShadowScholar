using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class JsonFile : MonoBehaviour
{
    [System.Serializable]
    public class ShadowScholar
    {
        public KinematicStart kinematicStart;
        public InputSettings inputSettings;
    }
    [System.Serializable]
    public class KinematicStart
    {
        public bool isFinish;
    }
    [System.Serializable]
    public class InputSettings
    {
        public string jumpInput;
        public string rollInput;
        public string sprintInput;
        public string crouchInput;
        public string strafeInput;
        public string weakAttackInput;
        public string strongAttackInput;
        public string blockInput;
        public string aimInput;
        public string shootInput;
        public string reloadInput;
        public string scopViewInput;
        public string switchCameraSideInput;
        public string openInventoryInput;
        public string closeInventoryInput;
        public string swimUpInput;
        public string swimDownInput;
        public string coverInput;
        public string exitZipLineInput;
        public string enterLadderInput;
        public string exitLadderInput;
        public string fastClimbInput;
        public string slideDownClimbInput;
        public string hideWeaponInput;
    }

    private string filePath;
    private string currentInputSettings;
    public ShadowScholar shadowScholar;
    [SerializeField] private bool isForInputSettings;
    [SerializeField] TMP_Text getTextInputSettings;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            ReadJsonFile();
        }
        else
        {
            CreateJsonFile();
        }
    }

    public void CreateJsonFile()
    {
        shadowScholar = new ShadowScholar()
        {
            kinematicStart = new KinematicStart()
            {
                isFinish = false
            },
            inputSettings = new InputSettings()
            {
                jumpInput = "Space",
                rollInput = "Q",
                sprintInput = "LeftShift",
                crouchInput = "C",
                strafeInput = "Tab",
                weakAttackInput = "Mouse0",
                strongAttackInput = "Alpha1",
                blockInput = "Mouse1",
                aimInput = "Mouse1",
                shootInput = "Mouse0",
                reloadInput = "R",
                scopViewInput = "Z",
                switchCameraSideInput = "Tab",
                openInventoryInput = "I",
                closeInventoryInput = "Escape",
                swimUpInput = "Space",
                swimDownInput = "LeftShift",
                coverInput = "Q",
                exitZipLineInput = "Space",
                enterLadderInput = "E",
                exitLadderInput = "Space",
                fastClimbInput = "LeftShift",
                slideDownClimbInput = "Q",
                hideWeaponInput = "H"
            }
        };

        SaveJson();
        Debug.Log("Fichier json crée");
    }

    public void ReadJsonFile()
    {
        string json = File.ReadAllText(filePath);
        shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);
    }

    void SaveJson()
    {
        string json = JsonUtility.ToJson(shadowScholar, true);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        Debug.Log("Données save");
    }

    public bool ReadKinematicStartJsonFile()
    {
        string json = File.ReadAllText(filePath);
        shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);
        Debug.Log("Boolean : " + shadowScholar.kinematicStart.isFinish);

        return shadowScholar.kinematicStart.isFinish;
    }

    public void UpdateKinematicStartDataJson(bool _isFinish)
    {
        if (shadowScholar != null)
        {
            shadowScholar.kinematicStart.isFinish = _isFinish;

            SaveJson();

            Debug.Log("Données update");
        } else { Debug.Log("shadowScholar est null"); }
    }

    public void HandleInputChange(string text)
    {
        currentInputSettings = text;
    }

    public void UpdateInputSettingsDataJson(string inputName)
    {
        if (shadowScholar != null)
        {
            string input = currentInputSettings;
            // permet de supprimer les caractère invisible
            input = input.Replace("\u200B", "");
            input = new string(input.Where(c => !char.IsControl(c) && c >= 32 && c <= 126).ToArray());
            PropertyInfo property = shadowScholar.inputSettings.GetType().GetProperty(inputName);

            if (property != null && property.CanWrite)
            {
                // Affectation dynamique de la valeur à la propriété
                property.SetValue(shadowScholar.inputSettings, input, null);
                SaveJson();
            }
            else
            {
                FieldInfo field = shadowScholar.inputSettings.GetType().GetField(inputName);

                if (field != null)
                {
                    // Affectation dynamique de la valeur au champ
                    field.SetValue(shadowScholar.inputSettings, input);
                    SaveJson();
                }
                else
                {
                    Debug.LogWarning($"Aucune propriété ou champ trouvé avec le nom '{inputName}'.");
                }
            }
        }
        else { Debug.Log("shadowScholar est null"); }
    }
}
