using Invector.vItemManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListenerQuit : MonoBehaviour
{
    [SerializeField] JsonFile jsonFile;
    [SerializeField] bool isMainMenu;
    [SerializeField] bool isPauseMenu;

    private GameObject player;
    private Transform playerTransform;
    private vItemManager vItemManager;

    private void Start()
    {
        if (isMainMenu)
        {
            jsonFile.shadowScholar.missions.isStart = false;
            jsonFile.SaveJson();
        }
    }

    private void Update()
    {
        if (Input.GetAxis("MoveZone") > 0)
        {
            SearchPlayerForSaveInventory();
        }
    }

    // save la position du joueur uniquement dans la scene Game et MenuPause ( si il est en pause en étant dans la scène game )
    private void SearchPlayerForSaveInventory()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                if (playerTransform != null)
                {
                    jsonFile.shadowScholar.player.position = playerTransform.position;
                    jsonFile.SaveJson();
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Game" ||
            SceneManager.GetActiveScene().name == "Mission1" ||
            SceneManager.GetActiveScene().name == "Mission2" ||
            SceneManager.GetActiveScene().name == "Mission3" ||
            SceneManager.GetActiveScene().name == "Mission4" ||
            SceneManager.GetActiveScene().name == "Mission5")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                vItemManager = player.GetComponent<vItemManager>();
                if (vItemManager != null)
                {
                    vItemManager.SaveInventory();
                }
            }
        }
    }

    // pour détecter le altf4
    void OnApplicationQuit()
    {
        if (jsonFile != null)
        {
            jsonFile.shadowScholar.missions.isStart = false;
            jsonFile.SaveJson();
        }
        SearchPlayerForSaveInventory();
    }

    // pour détecter le boutton quit du menu pause
    public void QuitGame()
    {
        Scene gameScene = SceneManager.GetSceneByName("Game");
        if (gameScene.isLoaded)
        {
            GameObject[] rootGameObjects = gameScene.GetRootGameObjects();
            foreach (GameObject obj in rootGameObjects)
            {
                if (obj.CompareTag("Player"))
                {
                    player = obj;
                    break;
                }
            }

            if (player != null)
            {
                playerTransform = player.transform;
                if (playerTransform != null)
                {
                    jsonFile.shadowScholar.player.position = playerTransform.position;
                    jsonFile.SaveJson();
                }

                vItemManager = player.GetComponent<vItemManager>();
                if (vItemManager != null)
                {
                    vItemManager.SaveInventory();
                }
            }
        }
        SearchPlayerForSaveInventory();
        Application.Quit();
    }
}