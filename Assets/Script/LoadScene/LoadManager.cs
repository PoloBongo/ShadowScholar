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
            // Commencer à charger la scène en arrière-plan //
            if (jsonFile != null)
            {
                if (jsonFile.shadowScholar.missions.isStart && !jsonFile.shadowScholar.missions.mission1.isFinish)
                {
                    StartCoroutine(PreloadSceneAndAssetsMission1("Mission1"));
                }
                else
                {
                    if (jsonFile.shadowScholar.missions.isStart && !jsonFile.shadowScholar.missions.mission2.isFinish)
                    {
                        StartCoroutine(PreloadSceneAndAssetsMission2("Mission2"));
                    }
                    else
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
        // Commencer à charger la scène en arrière-plan
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
        loadingText.text = "Préparation des assets nécessaire";
        // Charger la scène en arrière-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois chargé à 90% on peut activer la scène
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
        loadingText.text = $"Chargement de la scène : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la scène soit complètement activée pour passer à l'étape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme ça on peut charger les assets dedans
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
        // Instanciation des assets dans la nouvelle scène
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("map", null, 1, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Other", Roads, 2, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Zone_1", Zones, 3, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Zone_18", Zones, 4, 4));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enlève de force la scène de chargement pour éviter tout soucis
        loadingText.text = "Nettoyage de la scène de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneKinematicStart
    IEnumerator PreloadSceneAndAssetsKinematicStart(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Préparation des assets nécessaire";
        // Charger la scène en arrière-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois chargé à 90% on peut activer la scène
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
        loadingText.text = $"Chargement de la scène : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme ça on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        // empêche une superposition des ui
        GameObject transitionUI = GameObject.Find("TransitionUIForKinematic");
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
        // Instanciation des assets dans la nouvelle scène
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/Exterior", Zone_1, 1, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_3", Zone_1, 2, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_4", Zone_1, 3, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_4_1", Zone_1, 4, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_6_1", Zone_1, 5, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_7", Zone_1, 6, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_3", Zone_1, 7, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_3_1", Zone_1, 8, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_A_4", Zone_1, 9, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_B_6", Zone_1, 10, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_D_4", Zone_1, 11, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_E_1", Zone_1, 12, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/House_E_6", Zone_1, 13, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_1/School", Zone_1, 14, 34));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/Exterior", Zone_2, 15, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_4_3", Zone_2, 16, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_A_3", Zone_2, 17, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_B_4", Zone_2, 18, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_C_4_1", Zone_2, 19, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_D_3", Zone_2, 20, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_D_3_1", Zone_2, 21, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_2/House_E_4", Zone_2, 22, 34));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/Exterior", Zone_12, 23, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_A_1", Zone_12, 24, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_C_3", Zone_12, 25, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_E_2", Zone_12, 26, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_12/House_E_3", Zone_12, 27, 34));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_14/House_2", Zone_14, 28, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_14/Exterior", Zone_14, 29, 34));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/Exterior", Zone_15, 30, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/House_D_1", Zone_15, 31, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Kinematic/KinematicStart/Zone_15/House_E_6_1", Zone_15, 32, 34));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("map", null, 33, 34));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Other", Roads, 34, 34));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enlève de force la scène de chargement pour éviter tout soucis
        loadingText.text = "Nettoyage de la scène de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneMission1

    IEnumerator PreloadSceneAndAssetsMission1(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Préparation des assets nécessaire";
        // Charger la scène en arrière-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois chargé à 90% on peut activer la scène
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
        loadingText.text = $"Chargement de la scène : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la scène soit complètement activée pour passer à l'étape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme ça on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        GameObject transitionUI = GameObject.Find("TransitionUIForKinematic");
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
        // Instanciation des assets dans la nouvelle scène
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Basic_Hive", AllHub, 1, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Hub", AllHub, 2, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/MeleeCombat_Hive", AllHub, 3, 4));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission1/Shooter_Hive", AllHub, 4, 4));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enlève de force la scène de chargement pour éviter tout soucis
        loadingText.text = "Nettoyage de la scène de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    #region LoadSceneMission2

    IEnumerator PreloadSceneAndAssetsMission2(string sceneName)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingSlider.gameObject.SetActive(true);
        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        loadingText.text = "Préparation des assets nécessaire";
        // Charger la scène en arrière-plan sans l'activer
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        // Une fois chargé à 90% on peut activer la scène
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
        loadingText.text = $"Chargement de la scène : {operation.progress * 100:F0}%";
        operation.allowSceneActivation = true;

        // Attendre que la scène soit complètement activée pour passer à l'étape suivante
        while (!operation.isDone)
        {
            yield return null;
        }

        // on la met comme active comme ça on peut charger les assets dedans
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.IsValid())
        {
            SceneManager.SetActiveScene(targetScene);
        }

        GameObject transitionUI = GameObject.Find("TransitionUIForKinematic");
        if (transitionUI != null)
        {
            transitionUI.SetActive(false);
        }

        GameObject AllHub = GameObject.Find("AllHub");
        GameObject IA = GameObject.Find("IA");
        if (AllHub == null || IA == null)
        {
            Debug.LogError("GameObject missing in the scene.");
            yield break;
        }

        loadingText.color = new Color32(0xFF, 0xC2, 0x00, 0xFF);
        // Instanciation des assets dans la nouvelle scène
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Zone_25", AllHub, 1, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Prefabs Zone/Other", AllHub, 2, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("map", AllHub, 3, 11));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_1", IA, 4, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_2", IA, 5, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_3", IA, 6, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_4", IA, 7, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_5", IA, 8, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Agressif/vEnemyAI_Boss", IA, 9, 11));

        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Passif/vEnemyAI_1", IA, 10, 11));
        yield return StartCoroutine(LoadAndInstantiateAssetAsync("Mission2/IA_Passif/vEnemyAI_2", IA, 11, 11));

        if (transitionUI != null)
        {
            transitionUI.SetActive(true);
        }

        // on enlève de force la scène de chargement pour éviter tout soucis
        loadingText.text = "Nettoyage de la scène de chargement...";
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    #endregion
    IEnumerator LoadAndInstantiateAssetAsync(string assetPath, GameObject parentObject, int assetIndex, int totalAssets)
    {
        Debug.Log($"Attempting to load {assetPath}");

        // méthode synchrone pour charger un asset dans le dossier "Resources"
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
            // pour la progression du text en temps réel
            childrenLoaded++;
            float prefabProgress = (float)childrenLoaded / childrenCount * 100f;

            fakeProgress = Mathf.MoveTowards(prefabProgress, 1f, Time.deltaTime);
            loadingSlider.value = fakeProgress;
            loadingText.text = $"Chargement des assets : {assetIndex}/{totalAssets} - {prefabProgress:F0}%";
            // Attendre une frame pour chaque enfant instancié
            yield return null;
        }
    }
}