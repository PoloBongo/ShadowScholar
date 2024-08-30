using Invector;
using Invector.Utils;
using Invector.vCharacterController;
using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public string pauseMenuSceneName = "MenuPause";
    private List<MonoBehaviour> scriptsInMainScene = new List<MonoBehaviour>();

    private bool isPaused = false; 
    private vInventory inventory;


    // Update is called once per frame
    void Update()
    {
        inventory = FindAnyObjectByType<vInventory>();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                if (inventory != null)
                {
                    inventory.CloseInventory();
                }
                PauseScene();
            }
            else
            {
                ResumeScene();
            }
        }
    }

    void PauseScene()
    {
        // Trouver tous les objets actifs dans la scène principale
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            foreach (var script in obj.GetComponentsInChildren<MonoBehaviour>())
            {
                if(script.gameObject.name != "Pause" && script is not vDestroyGameObject)
                {
                    script.enabled = false;
                    scriptsInMainScene.Add(script);
                }
            }
        }

        GameObject.FindWithTag("Player").GetComponent<vThirdPersonController>().StopCharacter();

        StartCoroutine(LoadSceneAsync(pauseMenuSceneName));

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        isPaused = true;
    }

    public void ResumeScene()
    {
        StartCoroutine(UnLoadScene(pauseMenuSceneName));

        foreach (var script in scriptsInMainScene)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }

        scriptsInMainScene.Clear();

        Cursor.visible = false;

        isPaused = false;
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            yield return null;
        }


        foreach (GameObject obj in SceneManager.GetSceneByName(sceneName).GetRootGameObjects())
        {
            Button foundButton = obj.GetComponentInChildren<Button>(true);
            if (foundButton != null && foundButton.name == "ResumeBtn")
            {
                foundButton.onClick.AddListener(() => ResumeScene());
                Debug.Log("Btn Trouvé");
                break;
            }
        }
    }

    IEnumerator UnLoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
