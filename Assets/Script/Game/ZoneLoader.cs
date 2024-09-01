using Invector.vCamera;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoneLoader : MonoBehaviour
{

    public class Area
    {
        public string name;
        public GameObject obj;

        public Area(string _name, GameObject _obj)
        {
            name = _name;
            obj = _obj;
        }
    }


    public Canvas zoneMenuCanvas;
    private bool menuIsOpen;
    private bool isPressed;

    private GameObject player;
    private vShooterMeleeInput vShooterMeleeInput;
    private vThirdPersonController vThirdPersonController;
    private GameObject mainCamera;
    private vThirdPersonCamera vThirdPersonCamera;
    private Transform playerSpawn;

    private List<string> prefabPaths = new();
    private List<Area> loadedArea = new();
    private List<string> areaToLoad = new();
    private string playArea;
    private float loadingProgress = 0;
    private int ressourcesToLoad;

    public Canvas loadingScreenCanvas;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;

    private string filePath;
    public JsonFile jsonFile;

    public GenericInput openCloseMenuTPInput = new GenericInput("F2", "Start", "Start");

    // Start is called before the first frame update
    void Start()
    {
        zoneMenuCanvas.enabled = false;
        loadingScreenCanvas.enabled = false;

        filePath = Path.Combine(Application.persistentDataPath, "shadowScholar.json");

        if (File.Exists(filePath))
        {
            jsonFile.ReadJsonFile(filePath);
        }
    }

    // Start pour le player
    public void InitZoneLoader()
    {
        player = GameObject.FindWithTag("Player");
        vShooterMeleeInput = player.GetComponent<vShooterMeleeInput>();
        vThirdPersonController = player.GetComponent<vThirdPersonController>();
        mainCamera = GameObject.Find("vThirdPersonCamera");
        vThirdPersonCamera = mainCamera.GetComponent<vThirdPersonCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (openCloseMenuTPInput.GetButtonDown() && !isPressed)
        {
            if(!isPressed)
            {
                if (!menuIsOpen)
                {
                    OpenMenu();
                }
                else
                {
                    CloseMenu();
                }
                isPressed = true;
            }
        }

        if (openCloseMenuTPInput.GetButtonDown() && isPressed)
        {
            isPressed = false;
        }
    }

    // Ouvre le menu
    private void OpenMenu()
    {
        vShooterMeleeInput.SetLockAllInput(true);
        vThirdPersonController.StopCharacter();
        vThirdPersonCamera.FreezeCamera();
        zoneMenuCanvas.enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuIsOpen = true;
    }

    // Ferme le menu
    private void CloseMenu()
    {
        Cursor.visible = false;
        zoneMenuCanvas.enabled = false;
        vShooterMeleeInput.SetLockAllInput(false);
        vThirdPersonCamera.UnFreezeCamera();
        menuIsOpen = false;
    }

    // Zone de la maison
    public void ShowHouseZone()
    {

        areaToLoad.Add("Zone_14");
        StartCoroutine(ShowZones());
    }

    // Zone de l'université
    public void ShowSchoolZone()
    {
        areaToLoad.Add("Zone_1");
        StartCoroutine(ShowZones());
    }

    // Zone de l'agence 
    public void ShowAgencyZone()
    {
        areaToLoad.Add("Zone_18");
        StartCoroutine(ShowZones());
    }

    // Verifie que les resources sont chargés et instancie la zone
    IEnumerator LoadAndInstantiateZone(string _area_name)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>("Prefabs Zone/" + _area_name);

        while (!request.isDone)
        {
            yield return null;
        }

        GameObject prefab = (GameObject)request.asset;

        if (prefab != null)
        {
            GameObject area = Instantiate(prefab);
            loadedArea.Add(new Area(_area_name, area));
            if (_area_name == areaToLoad[0])
            {
                playerSpawn = area.transform.Find("AreaSpawn").transform;
            }
        }

    }

    // Chargent les ressources de façon séquencielle
    IEnumerator LoadRessourcesForZone(string _prefabs_path, string _area_name)
    {
        _prefabs_path = GetRelativePath(_prefabs_path, Application.dataPath + "Resources");
        _prefabs_path = Path.GetFileNameWithoutExtension(_prefabs_path);

        ResourceRequest request = Resources.LoadAsync<GameObject>("Prefabs Zone/" + _area_name + "/" + _prefabs_path);

        while (!request.isDone)
        {
            loadingSlider.value = loadingProgress + (request.progress * 100f / ressourcesToLoad) / areaToLoad.Count;
            loadingText.text = Mathf.RoundToInt(loadingProgress + (request.progress * 100f / ressourcesToLoad) / areaToLoad.Count) + "%";
            yield return null;
        }
        loadingProgress += (request.progress * 100f / ressourcesToLoad) / areaToLoad.Count; 

    }

    // Supprime les zones inutilisé et déchargent les resources
    IEnumerator UnLoadZone(Area _area)
    {
        DestroyImmediate(_area.obj, true);

        AsyncOperation operation = Resources.UnloadUnusedAssets();

        while (!operation.isDone)
        {
            yield return null;
        }

        loadedArea.Remove(_area);
    }

    // Charges les nouvelles zones 
    IEnumerator ShowZones()
    {
        loadingScreenCanvas.enabled = true;
        loadingProgress = 0;
        loadingSlider.value = 0;
        loadingText.text = "0%";

        playArea = areaToLoad[0];
        Area[] tempLoadedArea = loadedArea.ToArray();

        foreach (Area area in tempLoadedArea)
        {
            if (!areaToLoad.Contains(area.name))
            {
                yield return StartCoroutine(UnLoadZone(area));
            }
        }

        foreach (string area in areaToLoad)
        {
            if (!CheckContains(loadedArea, area))
            {
                string[] prefabsPath = Directory.GetFiles(Path.Combine(Application.dataPath, "Resources", "Prefabs Zone/" + area), "*.prefab", SearchOption.TopDirectoryOnly);
                ressourcesToLoad = prefabsPath.Length;
                foreach (string prefabPath in prefabsPath)
                {
                    yield return StartCoroutine(LoadRessourcesForZone(prefabPath, area));
                }
                yield return StartCoroutine(LoadAndInstantiateZone(area));
            }
        }


        if (areaToLoad.Count == loadedArea.Count)
        {
            player.transform.SetPositionAndRotation(playerSpawn.position, playerSpawn.rotation);

            areaToLoad.Clear();

            jsonFile.shadowScholar.area.playArea = playArea;
            jsonFile.shadowScholar.area.playerTransform = playerSpawn;
            jsonFile.SaveJson();

            player.GetComponent<Activation>().ActivateScript();

            CloseMenu();
            loadingScreenCanvas.enabled = false;
        }

    }

    // Check si la list contient un certain nom
    private bool CheckContains(List<Area> loadedList, string _area)
    {
        foreach (Area area in loadedList)
        {
            if (area.name == _area)
            {
                return true;
            }
        }
        return false;
    }

    // Renvoie le chemin relatif d'une resources 
    private string GetRelativePath(string fullPath, string basePath)
    {

        string relativePath = Path.GetRelativePath(basePath, fullPath);
        return relativePath.Replace('\\', '/');
    }
}
