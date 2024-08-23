using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    public Text loadingText;
    public Slider loadingSlider;
    [SerializeField] JsonFile jsonFile;
    [SerializeField] bool isNormalGameplay;
    private int sceneIndex;

    private void Start()
    {
        if (isNormalGameplay)
        {
            // Commencer � charger la sc�ne en arri�re-plan //
            if (jsonFile != null)
            {
                if (jsonFile.ReadMission1IsStartJsonFile() && !jsonFile.shadowScholar.missions.mission1.isFinish)
                {
                    StartCoroutine(PreloadSceneAndAssetsMission1("Mission1"));
                }
                else
                {
                    if (jsonFile.ReadMission2IsStartJsonFile() && !jsonFile.shadowScholar.missions.mission2.isFinish)
                    {
                        StartCoroutine(PreloadSceneAndAssetsMission2("Mission2"));
                    } else
                    {
                        if (jsonFile.ReadKinematicStartJsonFile())
                        {
                            StartCoroutine(PreloadSceneAndAssetsGame("Game"));
                        }
                        else
                        {
                            StartCoroutine(PreloadSceneAndAssetsKinematicStart("Kinematic"));
                        }
                    }
                }
            }
        }
    }

    public void StartLoadManager()
    {
        // Commencer � charger la sc�ne en arri�re-plan
        if (jsonFile != null)
        {
            if (jsonFile.ReadKinematicStartJsonFile())
            {
                StartCoroutine(PreloadSceneAndAssetsGame("Game"));
            }
            else
            {
                StartCoroutine(PreloadSceneAndAssetsKinematicStart("Kinematic"));
            }
        }
    }

    #region LoadSceneGame
    IEnumerator PreloadSceneAndAssetsGame(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Pr�paration des assets n�cessaire";
        // Charger la sc�ne en arri�re-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois charg� � 90% on peut activer la sc�ne
        while (operation.progress < 0.9f)
        {
            if (operation.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }
        loadingSlider.value = 90f;

        loadingText.color = Color.red;
        loadingText.text = $"Chargement de la sc�ne : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la sc�ne soit compl�tement activ�e pour passer � l'�tape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme �a on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        GameObject transitionUI = GameObject.Find("TransitionUI");
        if (transitionUI != null)
        {
            transitionUI.SetActive(false);
        }

        GameObject Zones = GameObject.Find("Zones");
        GameObject Roads = GameObject.Find("Roads");
        if (Zones == null || Roads == null)
        {
            Debug.LogError("GameObject missing in the scene.");
            yield break;
        }

        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        // Instanciation des assets dans la nouvelle sc�ne
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("map", null, 1, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Other", Roads, 2, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Zone_1", Zones, 3, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Zone_18", Zones, 4, 4));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enl�ve de force la sc�ne de chargement pour �viter tout soucis
        loadingText.text = "Nettoyage de la sc�ne de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneKinematicStart
    IEnumerator PreloadSceneAndAssetsKinematicStart(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Pr�paration des assets n�cessaire";
        // Charger la sc�ne en arri�re-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois charg� � 90% on peut activer la sc�ne
        while (operation.progress < 0.9f)
        {
            if (operation.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }
        loadingSlider.value = 90f;

        loadingText.color = Color.red;
        loadingText.text = $"Chargement de la sc�ne : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la sc�ne soit compl�tement activ�e pour passer � l'�tape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme �a on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        // emp�che une superposition des ui
        GameObject transitionUI = GameObject.Find("TransitionUI");
        if (transitionUI != null)
        {
            transitionUI.SetActive(false);
        }

        GameObject Zone_1 = GameObject.Find("Zone_1");
        GameObject Zone_2 = GameObject.Find("Zone_2");
        GameObject Zone_12 = GameObject.Find("Zone_12");
        GameObject Zone_14 = GameObject.Find("Zone_14");
        GameObject Zone_15 = GameObject.Find("Zone_15");
        GameObject Roads = GameObject.Find("Roads");
        if (Zone_1 == null || Zone_2 == null || Zone_14 == null || Zone_15 == null || Zone_12 == null || Roads == null)
        {
            Debug.LogError("GameObject missing in the scene.");
            yield break;
        }

        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        // Instanciation des assets dans la nouvelle sc�ne
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/Exterior", Zone_1, 1, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_3", Zone_1, 2, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_4", Zone_1, 3, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_4_1", Zone_1, 4, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_6_1", Zone_1, 5, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_7", Zone_1, 6, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_3", Zone_1, 7, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_3_1", Zone_1, 8, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_4", Zone_1, 9, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_B_6", Zone_1, 10, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_D_4", Zone_1, 11, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_E_1", Zone_1, 12, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_E_6", Zone_1, 13, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/School", Zone_1, 14, 35));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/Exterior", Zone_2, 15, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_4_3", Zone_2, 16, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_A_3", Zone_2, 17, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_B_4", Zone_2, 18, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_C_4_1", Zone_2, 19, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_D_3", Zone_2, 20, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_D_3_1", Zone_2, 21, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_E_4", Zone_2, 22, 35));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/Exterior", Zone_12, 23, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_A_1", Zone_12, 24, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_C_3", Zone_12, 25, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_E_2", Zone_12, 26, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_E_3", Zone_12, 27, 35));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_14/House_2", Zone_14, 28, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_14/Exterior", Zone_14, 29, 35));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/Exterior", Zone_15, 30, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/House_D_1", Zone_15, 31, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/House_E_6_1", Zone_15, 32, 35));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("map", null, 33, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Other", Roads, 34, 35));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/KinematicController", null, 35, 35));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enl�ve de force la sc�ne de chargement pour �viter tout soucis
        loadingText.text = "Nettoyage de la sc�ne de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneMission1

    IEnumerator PreloadSceneAndAssetsMission1(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Pr�paration des assets n�cessaire";
        // Charger la sc�ne en arri�re-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois charg� � 90% on peut activer la sc�ne
        while (operation.progress < 0.9f)
        {
            if (operation.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }
        loadingSlider.value = 90f;

        loadingText.color = Color.red;
        loadingText.text = $"Chargement de la sc�ne : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la sc�ne soit compl�tement activ�e pour passer � l'�tape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme �a on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        GameObject transitionUI = GameObject.Find("TransitionUI");
        if (transitionUI != null)
        {
            transitionUI.SetActive(false);
        }

        GameObject AllHub = GameObject.Find("AllHub");
        if (AllHub == null)
        {
            Debug.LogError("GameObject missing in the scene.");
            yield break;
        }

        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        // Instanciation des assets dans la nouvelle sc�ne
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Basic_Hive", AllHub, 1, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Hub", AllHub, 2, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/MeleeCombat_Hive", AllHub, 3, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Shooter_Hive", AllHub, 4, 4));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enl�ve de force la sc�ne de chargement pour �viter tout soucis
        loadingText.text = "Nettoyage de la sc�ne de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneMission2

    IEnumerator PreloadSceneAndAssetsMission2(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Pr�paration des assets n�cessaire";
        // Charger la sc�ne en arri�re-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois charg� � 90% on peut activer la sc�ne
        while (operation.progress < 0.9f)
        {
            if (operation.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }
        loadingSlider.value = 90f;

        loadingText.color = Color.red;
        loadingText.text = $"Chargement de la sc�ne : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la sc�ne soit compl�tement activ�e pour passer � l'�tape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme �a on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        GameObject transitionUI = GameObject.Find("TransitionUI");
        if (transitionUI != null)
        {
            transitionUI.SetActive(false);
        }

        GameObject AllHub = GameObject.Find("AllHub");
        if (AllHub == null)
        {
            Debug.LogError("GameObject missing in the scene.");
            yield break;
        }

        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        // Instanciation des assets dans la nouvelle sc�ne
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/", AllHub, 1, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/", AllHub, 2, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/", AllHub, 3, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/", AllHub, 4, 4));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enl�ve de force la sc�ne de chargement pour �viter tout soucis
        loadingText.text = "Nettoyage de la sc�ne de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    IEnumerator LoadAndInstantiateAssetAsync(string assetPath, GameObject parentObject, int assetIndex, int totalAssets)
    {
        Debug.Log($"Attempting to load {assetPath}");

        // m�thode synchrone pour charger un asset dans le dossier "Resources"
        GameObject prefab = Resources.Load<GameObject>(assetPath);

        if (prefab == null)
        {
            Debug.LogError($"Failed to load asset at path: {assetPath}. Make sure the asset is in a Resources folder.");
            yield break;
        }

        Debug.Log("Asset loaded successfully.");

        int childrenCount = prefab.transform.childCount;
        int childrenLoaded = 0;
        float fakeProgress = 0f;
        Instantiate(prefab, parentObject != null ? parentObject.transform : null);
        foreach (Transform child in prefab.transform)
        {
            // pour la progression du text en temps r�el
            childrenLoaded++;
            float prefabProgress = (float)childrenLoaded / childrenCount * 100f;

            fakeProgress = Mathf.MoveTowards(prefabProgress, 1f, Time.deltaTime);
            loadingSlider.value = fakeProgress;
            loadingText.text = $"Chargement des assets : {assetIndex}/{totalAssets} - {prefabProgress:F0}%";
            // Attendre une frame pour chaque enfant instanci�
            yield return null;
        }
    }
}