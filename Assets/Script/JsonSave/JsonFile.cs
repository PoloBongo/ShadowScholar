using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JsonFile : MonoBehaviour
{
    [System.Serializable]
    public class ShadowScholar
    {
        public KinematicStart kinematicStart;
        public InputSettings inputSettings;
        public Horloge horloge;
        public Missions missions;
        public Area area;
        public Player player;
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
        public string scopeViewInput;
        public string switchCameraSideInput;
        public string openInventoryInput;
        public string closeInventoryInput;
        public string swimUpInput;
        public string swimDownInput;
        public string coverInput;
        public string exitZipLineInput;
        public string enterInput;
        public string exitInput;
        public string fastClimbInput;
        public string slideDownClimbInput;
        public string hideWeaponInput;
        public string openObjectifInput;
        public string closeObjectifInput;
        public string openCloseMenuTPInput;
    }
    [System.Serializable]
    public class Horloge
    {
        public float time;
    }

    [System.Serializable]
    public class Missions
    {
        public Mission1 mission1;
        public Mission2 mission2;
        public Mission3 mission3;
        public bool isStart;
    }

    [System.Serializable]
    public class Mission1
    {
        public bool isFinish;
    }

    [System.Serializable]
    public class Mission2
    {
        public bool isFinish;
    }
    
    [System.Serializable]
    public class Mission3
    {
        public bool isFinish;
    }

    [System.Serializable]
    public class Area
    {
        public string playArea;
        public Transform playerTransform;
    }

    [System.Serializable]
    public class Player
    {
        public Vector3 position;
    }

    private string filePath;
    public ShadowScholar shadowScholar;
    [SerializeField] private bool isForInputSettings;
    [SerializeField] private List<TMP_InputField> inputFields;
    private TMP_InputField activeInputField;
    private bool isListeningForKey = false;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            ReadJsonFile(filePath);
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
                scopeViewInput = "Z",
                switchCameraSideInput = "Tab",
                openInventoryInput = "I",
                closeInventoryInput = "Escape",
                swimUpInput = "Space",
                swimDownInput = "LeftShift",
                coverInput = "Q",
                exitZipLineInput = "Space",
                enterInput = "E",
                exitInput = "Space",
                fastClimbInput = "LeftShift",
                slideDownClimbInput = "Q",
                hideWeaponInput = "H",
                openObjectifInput = "F1",
                closeObjectifInput = "Escape",
                openCloseMenuTPInput = ";"
            },
            horloge = new Horloge()
            {
                time = 0f
            },
            missions = new Missions()
            {
                mission1 = new Mission1()
                {
                    isFinish = false
                },
                mission2 = new Mission2()
                {
                    isFinish = false
                },
                mission3 = new Mission3()
                {
                    isFinish = false
                },
                isStart = false
            },
            area = new Area()
            {
                playArea = "Zone_14",
                playerTransform = null
            },
            player = new Player()
            {
                position = new Vector3(1459.83f, 16.873f, 1011.18f)
            }
        };

        SaveJson();
        Debug.Log("Fichier json crée");
    }

    public void ReadJsonFile(string filePath)
    {
        if (filePath == "")
        {
            return;
        }
        else
        {
            string json = File.ReadAllText(filePath);
            shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);
        }
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(shadowScholar, true);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        Debug.Log("Données save");
    }

    public bool ReadKinematicStartJsonFile()
    {
        string json = File.ReadAllText(filePath);
        shadowScholar = JsonUtility.FromJson<ShadowScholar>(json);

        return shadowScholar.kinematicStart.isFinish;
    }

    public void UpdateKinematicStartDataJson(bool _isFinish)
    {
        if (shadowScholar != null)
        {
            shadowScholar.kinematicStart.isFinish = _isFinish;
            SaveJson();
        } else { Debug.Log("shadowScholar est null"); }
    }

    public void DetectInputFieldSettings()
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.onSelect.AddListener(delegate { StartListeningForKey(inputField); });
            inputField.onDeselect.AddListener(delegate { StopListeningForKey(); });
        }
    }

    void Update()
    {
        if (isListeningForKey)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    if (keyCode != KeyCode.Mouse0 && keyCode != KeyCode.Mouse1 && keyCode != KeyCode.Mouse2)
                    {
                        string keyName = keyCode.ToString().ToUpper();
                        if (IsSpecialKey(keyCode))
                        {
                            activeInputField.text = keyName;
                        }

                        string inputFieldName = activeInputField.gameObject.name;
                        UpdateInputSettingsDataJson(inputFieldName, keyCode.ToString().ToUpper());
                        EventSystem.current.SetSelectedGameObject(null);
                        StopListeningForKey();
                        break;
                    }
                }
            }
        }
    }

    private bool IsSpecialKey(KeyCode keyCode)
    {
        return keyCode == KeyCode.Space ||
               keyCode == KeyCode.Backspace ||
               keyCode == KeyCode.Escape ||
               keyCode == KeyCode.F1 ||
               keyCode == KeyCode.F2 ||
               keyCode == KeyCode.F3 ||
               keyCode == KeyCode.F4 ||
               keyCode == KeyCode.F5 ||
               keyCode == KeyCode.F6 ||
               keyCode == KeyCode.F7 ||
               keyCode == KeyCode.F8 ||
               keyCode == KeyCode.F9 ||
               keyCode == KeyCode.F10 ||
               keyCode == KeyCode.F11 ||
               keyCode == KeyCode.F12;
    }

    private void StartListeningForKey(TMP_InputField inputField)
    {
        isListeningForKey = true;
        activeInputField = inputField;
    }

    private void StopListeningForKey()
    {
        isListeningForKey = false;
        activeInputField = null;
    }

    public void UpdateInputSettingsDataJson(string inputName, string inputContent)
    {
        if (shadowScholar != null)
        {
            string input = inputContent;
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
