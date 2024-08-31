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
    private class ScriptState
    {
        public MonoBehaviour script;
        public bool stockEnabled;
    }

    public string pauseMenuSceneName = "MenuPause";
    private List<ScriptState> scriptsInMainScene = new List<ScriptState>();

    private bool isPaused = false; 
    private vInventory inventory;
    private GameObject player;
    private GameObject gameController;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventory = FindAnyObjectByType<vInventory>();
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
                    scriptsInMainScene.Add(new ScriptState
                    {
                        script = script,
                        stockEnabled = script.enabled
                    });
                    script.enabled = false;
                }
            }
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            vThirdPersonController playerController = playerObject.GetComponent<vThirdPersonController>();
            if (playerController != null)
            {
                playerController.StopCharacter();
            }
        }

        StartCoroutine(LoadSceneAsync(pauseMenuSceneName));

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        isPaused = true;
    }

    private void GetInfo()
    {
        gameController = GameObject.FindWithTag("GameController");
        player = GameObject.FindWithTag("Player");
    }

    public void DebugMe()
    {
        GetInfo();
        if (player != null &&  gameController != null)
        {
            player.transform.position = gameController.GetComponent<vGameController>().DebugPoint.position;
        }
    }

    public void ResumeScene()
    {
        StartCoroutine(UnLoadScene(pauseMenuSceneName));

        foreach (var script in scriptsInMainScene)
        {
            if (script.script != null && script.stockEnabled)
            {
                script.script.enabled = true;
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
