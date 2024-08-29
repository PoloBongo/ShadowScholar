using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveComponentByScene : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    [SerializeField] private List<string> sceneName = new List<string>();
    void Start()
    {
        string sceneNameActive = SceneManager.GetActiveScene().name;
        for (int i = 0; i < sceneName.Count; i++)
        {
            if (sceneName[i] == sceneNameActive)
            {
                if (TryGetComponent<NavMeshSurface>(out navMeshSurface))
                {
                    navMeshSurface.enabled = true;
                }
            }
        }
    }
}
